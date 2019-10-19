using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using WebApi.Models;
using WebApi.MyDBContext;
using Firebase.Database;
using Firebase.Database.Query;
using WebApi.ModelsFirebase;
using WebApi.ViewModels;

namespace WebApi.Controllers
{
    [RoutePrefix("api/MyApi")]
    [Authorize]
    public class MyController : ApiController
    {
        private MyDbContext context;
        private FirebaseClient firebaseClient;
        private static string FRIEND_REQUEST = "friend_request";
        private static string ROOM_REQUEST = "room_request";
        private static string ROOM = "room";

        public MyController()
        {
            context = MyDbContext.Create();
            firebaseClient = new FirebaseClient("https://luu-data.firebaseio.com/");
        }

        #region user
        [HttpPost]
        [Route("UpdateInfomation")]
        public async Task<IHttpActionResult> UpdateInfomation(InfomationUerModel userModel)
        {
            ApplicationUser user = context.Users.Find(userModel.UserId);
            if (user == null)
            {
                return NotFound();
            }
            if (userModel.FullName != null)
            {
                user.FullName = userModel.FullName;
            }
            if (userModel.Address != null)
            {
                user.Address = userModel.Address;
            }
            if (userModel.DoB != null)
            {
                user.DoB = userModel.DoB;
            }
            if (userModel.Phone != null)
            {
                user.Phone = userModel.Phone;
            }
            await context.SaveChangesAsync();
            return Ok();
        }

        [Route("UpdateAvatar")]
        [HttpGet]
        public async Task<IHttpActionResult> UpdateAvatar(string userId, string pathAvatar)
        {
            ApplicationUser user = context.Users.Find(userId);
            if (user == null)
            {
                return NotFound();
            }
            user.Avatar = pathAvatar;
            await context.SaveChangesAsync();
            return Ok();
        }

        [Route("FindUserByName")]
        [HttpGet]
        public async Task<ICollection<ApplicationUser>> FindUserByName(string name)
        {
            ICollection<ApplicationUser> listUser =
            context.Users.ToList().Where(t => t.FullName.ToLower().Contains(name.ToLower())).ToList();
            return listUser;
        }

        public async Task<ICollection<ApplicationUser>> GetListUser()
        {
            Random rnd = new Random();
            return context.Users.OrderBy(t => rnd.Next()).ToList();
        }
        #endregion

        #region friend request
        [Route("SendFriendRequest")]
        [HttpPost]
        public async Task<IHttpActionResult> SendFriendRequest(FriendRequest friendRequest)
        {
            //kiểm tra đã gửi chưa
            var haveSend = context.FriendRequests.FirstOrDefault(t => t.UserSend == friendRequest.UserSend && t.UserId == friendRequest.UserId);
            if (haveSend != null)
            {
                return BadRequest("already send");
            }
            //kiểm tra người kia có phải đã gửi lời mời trước rồi ko, nếu có => đồng ý
            var userSend = context.FriendRequests.FirstOrDefault(t => t.UserSend == friendRequest.UserId && t.UserId == friendRequest.UserSend);
            if (userSend != null)
            {
                await ReplyFriendRequest(userSend.Id, true);
            }


            context.FriendRequests.Add(friendRequest);
            FriendRequestFB friendRequestFB = new FriendRequestFB()
            {
                ID = friendRequest.Id.ToString(),
                IDUser = friendRequest.UserId,
                IDUserSend = friendRequest.UserSend,
                Content = friendRequest.Content,
                TimeSend = DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss")
            };
            var result = firebaseClient
                .Child(FRIEND_REQUEST)
                .Child(friendRequest.Id.ToString())
                .PutAsync(friendRequestFB);

            if (!result.IsCompleted)
            {
                return BadRequest();
            }

            await context.SaveChangesAsync();
            return Ok();
        }

        [Route("DeleteFriendRequest")]
        [HttpPost]
        public async Task<IHttpActionResult> DeleteFriendRequest([FromBody]Guid id)
        {
            var fRequest = context.FriendRequests.FirstOrDefault(t => t.Id == id);
            if (fRequest != null)
            {
                await firebaseClient.Child(FRIEND_REQUEST)
                 .Child(id.ToString())
                 .DeleteAsync();
                context.FriendRequests.Remove(fRequest);
                await context.SaveChangesAsync();
                await firebaseClient
                    .Child(FRIEND_REQUEST)
                    .Child(fRequest.Id.ToString())
                    .DeleteAsync();
            }
            return Ok();
        }

        [Route("ReplyFriendRequest")]
        [HttpPost]
        public async Task<IHttpActionResult> ReplyFriendRequest([FromBody]Guid idFriendRequest, [FromBody] bool isAccept)
        {
            var fr = context.FriendRequests.FirstOrDefault(t => t.Id == idFriendRequest);
            if (fr == null)
            {
                return NotFound();
            }
            if (isAccept)
            {
                await AddFriend(fr.UserSend, fr.UserId);
            }
            await DeleteFriendRequest(fr.Id);
            await context.SaveChangesAsync();
            return Ok();
        }

        #endregion

        #region Room
        [Route("AddRoom")]
        [HttpPost]
        public async Task<IHttpActionResult> AddRoom([FromBody] AddRoomViewModel addRoomViewModel)
        {
            Room room = new Room()
            {
                ColorRoom = "blue",
                StickerRoom = "like",
                PathAvatar = "",
                NameRoom = ""
            };
            ApplicationUser user = context.Users.Find(addRoomViewModel.UserCreate);
            if (user == null)
            {
                return BadRequest();
            }
            context.Rooms.Add(room);
            foreach (var item in addRoomViewModel.ListUserId)
            {
                var u = context.Users.Find(item);
                if (u != null)
                {
                    UserJoinRoom userJoinRoom = new UserJoinRoom()
                    {
                        Room = room,
                        User = u,
                        UserId = item
                    };
                    context.UserJoinRooms.Add(userJoinRoom);
                }
            }
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        [Route("UpdateRoom")]
        public async Task<IHttpActionResult> UpdateRoom(Room room)
        {
            Room oldRoom = context.Rooms.Find(room.Id);
            if (oldRoom == null)
            {
                return NotFound();
            }
            oldRoom.KeyCall = room.KeyCall;
            oldRoom.KeyVideoCall = room.KeyVideoCall;
            oldRoom.StickerRoom = room.StickerRoom;
            oldRoom.ColorRoom = room.ColorRoom;
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        [Route("RemoveUserFromRoom")]
        public async Task<IHttpActionResult> RemoveUserFromRoom([FromBody] string userId, [FromBody] Guid idRoom)
        {
            UserJoinRoom userJoinRoom = context.UserJoinRooms.Where(t => t.UserId == userId && t.Room.Id == idRoom).First();
            if (userJoinRoom != null)
            {
                context.UserJoinRooms.Remove(userJoinRoom);
            }
            await context.SaveChangesAsync();
            return Ok();
        }

        [Route("GetRoom")]
        [HttpGet]
        public Room GetRoom(Guid idRoom)
        {
            var room = context.Rooms.Find(idRoom);
            return room;
        }
        #endregion

        #region Emoji
        [AllowAnonymous]
        [Route("AddEmoji")]
        [HttpGet]
        public async Task<IHttpActionResult> AddEmoji(string character, string pathEmoji, Guid idType)
        {
            if (context.Emoji.FirstOrDefault(t => t.NameEmoji.ToLower().Equals(character.ToLower())) == null)
            {
                var emoji = new Emoji
                {
                    NameEmoji = character,
                    PathImage = pathEmoji,
                    IdType = idType
                };
                context.Emoji.Add(emoji);

                await context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }

        [AllowAnonymous]
        [Route("RemoveEmoji")]
        [HttpGet]
        public async Task<IHttpActionResult> RemoveEmoji(Guid id)
        {
            Emoji emoji = context.Emoji.Find(id);
            if (emoji != null)
            {
                context.Emoji.Remove(emoji);
            }
            await context.SaveChangesAsync();
            return Ok();
        }

        [AllowAnonymous]
        [Route("AddTypeEmoji")]
        [HttpGet]
        public async Task<IHttpActionResult> AddTypeEmoji(string name, string path)
        {
            if (context.TypeEmojis.FirstOrDefault(t => t.NameType.ToLower().Equals(name.ToLower())) == null)
            {
                var emoji = new TypeEmoji
                {
                    NameType = name,
                    PathThumbnail = path
                };
                context.TypeEmojis.Add(emoji);

                await context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }

        [AllowAnonymous]
        [Route("RemoveTypeEmoji")]
        [HttpGet]
        public async Task<IHttpActionResult> RemoveTypeEmoji(string name)
        {
            TypeEmoji emoji = context.TypeEmojis.FirstOrDefault(t => t.NameType.ToLower().Equals(name.ToLower()));
            if (emoji != null)
            {
                context.TypeEmojis.Remove(emoji);
            }
            await context.SaveChangesAsync();
            return Ok();
        }

        [AllowAnonymous]
        [Route("GetEmoji")]
        public async Task<Emoji> GetEmoji(string character)
        {
            Emoji emoji = context.Emoji.FirstOrDefault(t => t.NameEmoji.ToLower().Equals(character.ToLower()));
            return emoji;
        }

        [AllowAnonymous]
        [Route("GetAllEmoji")]
        public async Task<List<Emoji>> GetAllEmoji()
        {
            List<Emoji> emojis = context.Emoji.ToList();
            return emojis;
        }
        #endregion

        #region Block User
        [Route("BlockUser")]
        public async Task<IHttpActionResult> BlockUser(string idFromUser, string idUserBlock)
        {
            var blockUser = context.BlockUsers.FirstOrDefault(t => t.UserID.Equals(idFromUser) && t.UserBlockId.Equals(idUserBlock));
            if (blockUser == null)
            {
                BlockUser block = new BlockUser()
                {
                    UserID = idFromUser,
                    UserBlockId = idUserBlock
                };
                context.BlockUsers.Add(block);
                await context.SaveChangesAsync();
            }
            return Ok();
        }

        [Route("RemoveBlockUser")]
        public async Task<IHttpActionResult> RemoveBlockUser(string idFromUser, string idUserBlock)
        {
            var blockUser = context.BlockUsers.FirstOrDefault(t => t.UserID.Equals(idFromUser) && t.UserBlockId.Equals(idUserBlock));
            if (blockUser != null)
            {
                context.BlockUsers.Remove(blockUser);
                await context.SaveChangesAsync();
            }
            return Ok();
        }

        [Route("GetAllBlockByIdUser")]
        [HttpGet]
        public async Task<IList<BlockUser>> GetAllBlockByIdUser(string id)
        {
            var listBlock = context.BlockUsers.Where(t => t.UserID.Equals(id)).ToList();
            return listBlock;
        }
        #endregion

        #region Friend
        [Route("AddFriend")]
        public async Task<IHttpActionResult> AddFriend(string idUser1, string idUser2)
        {
            var userExist = context.Friends.FirstOrDefault(t => (t.User1Id.Equals(idUser1) && t.User2Id.Equals(idUser2))
            || (t.User1Id.Equals(idUser2) && t.User2Id.Equals(idUser1)));
            if (userExist == null)
            {
                var friend = new Friend()
                {
                    User1Id = idUser1,
                    User2Id = idUser2,
                    LastInterractive = DateTime.Now
                };
                context.Friends.Add(friend);
                await context.SaveChangesAsync();
            }
            return Ok();
        }

        [Route("RemoveFriend")]
        [HttpPost]
        public async Task<IHttpActionResult> RemoveFriend([FromBody]string idUser1, [FromBody] string idUser2)
        {
            var friend = context.Friends
                .FirstOrDefault(t => (t.User1Id == idUser1 && t.User2Id == idUser2)
                        || (t.User1Id == idUser2 && t.User2Id == idUser1));
            if (friend == null)
            {
                return NotFound();
            }
            context.Friends.Remove(friend);
            await context.SaveChangesAsync();
            return Ok();
        }

        [Route("GetAllFriend")]
        [HttpGet]
        public async Task<IList<Friend>> GetAllFriend(string idUser)
        {
            var friends = context.Friends
                .Where(t => t.User1Id.Equals(idUser) || t.User2.Equals(idUser)).ToList();
            return friends;
        }
        #endregion

        #region room request
        [HttpPost]
        [Route("SendRoomRequest")]
        public async Task<IHttpActionResult> SendRoomRequest([FromBody]string userSend, [FromBody]string user, [FromBody] string content)
        {
            var roomRq = context.RoomRequests.FirstOrDefault(t => t.UserId.Equals(user) && t.UserSendID.Equals(userSend));
            if (roomRq == null)
            {
                var rq = new RoomRequest()
                {
                    UserSendID = userSend,
                    UserId = user,
                    Content = content
                };
                context.RoomRequests.Add(rq);
                await context.SaveChangesAsync();

                var rqbf = new RoomRequestFB()
                {
                    ID = rq.Id.ToString(),
                    IDUserSend = rq.UserSendID,
                    IDUser = rq.UserId,
                    Content = rq.Content,
                    TimeSend = DateTime.UtcNow.ToString("mm/dd/yyyy hh:mm:ss")
                };
                var result = firebaseClient
                    .Child(ROOM_REQUEST)
                    .Child(rq.Id.ToString())
                    .PutAsync(rqbf);
            }
            return Ok();
        }

        [Route("RemoveRoomRequest")]
        [HttpGet]
        public async Task<IHttpActionResult> RemoveRoomRequest(Guid id)
        {
            var rq = context.RoomRequests.Find(id);
            if (rq != null)
            {
                context.RoomRequests.Remove(rq);
                await context.SaveChangesAsync();
                await firebaseClient.Child(ROOM_REQUEST)
                                            .Child(rq.Id.ToString())
                                            .DeleteAsync();
            }
            return Ok();
        }

        [Route("GetAllRoomRequest")]
        [HttpGet]
        public async Task<IList<RoomRequest>> GetAllRoomRequest(string id)
        {
            var listRoomRequest = context.RoomRequests.Where(t => t.UserId.Equals(id)).ToList();
            return listRoomRequest;
        }
        #endregion

        #region User Join Room
        [HttpPost]
        [Route("AddUserToRoom")]
        public async Task<IHttpActionResult> AddUserToRoom([FromBody]string idUser, [FromBody] Guid idRoom)
        {
            var joinExist = context.UserJoinRooms.FirstOrDefault(t => t.UserId.Equals(idUser) && t.RoomId == idRoom);
            var user = context.Users.Find(idUser);
            if (joinExist == null)
            {
                var userJoinRoom = new UserJoinRoom()
                {
                    UserId = idUser,
                    RoomId = idRoom,
                    NickName = user.FullName
                };
                context.UserJoinRooms.Add(userJoinRoom);
                await context.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpPost]
        [Route("RemoveUserRoom")]
        public async Task<IHttpActionResult> RemoveUserRoom([FromBody] string idUser, [FromBody] Guid idRoom)
        {
            var joinExist = context.UserJoinRooms.FirstOrDefault(t => t.UserId.Equals(idUser) && t.RoomId == idRoom);
            if (joinExist != null)
            {
                context.UserJoinRooms.Remove(joinExist);
                await context.SaveChangesAsync();
            }
            return Ok();
        }


        [Route("GetListAvatarRoom")]
        [HttpGet]
        public IList<string> GetListAvatarRoom(Guid idRoom)
        {
            var room = context.Rooms.Find(idRoom);
            var list = new List<string>();
            if (!room.PathAvatar.Equals(""))
            {
                list.Add(room.PathAvatar);
            }
            else
            {
                var listJoinRoom = context.UserJoinRooms.Where(t => t.RoomId == idRoom).Select(t => t.UserId).ToList();
                foreach (var item in listJoinRoom)
                {
                    var u = context.Users.Find(item);
                    list.Add(u.Avatar);
                }
            }
            return list;
        }

        [Route("GetListUserJoinRoom")]
        [HttpGet]
        public IList<ApplicationUser> GetListUserJoinRoom(Guid idRoom)
        {
            var listUser = new List<ApplicationUser>();
            var listJoinRoom = context.UserJoinRooms.Where(t => t.RoomId == idRoom).Select(t => t.UserId).ToList();
            foreach (var item in listJoinRoom)
            {
                var u = context.Users.Find(item);
                listUser.Add(u);
            }
            return listUser;
        }
        #endregion

        #region Content Chat
        [HttpPost]
        [Route("SendContentChat")]
        public async Task<IHttpActionResult> SendContentChat([FromBody]ContentChatViewModel contentChatViewModel)
        {
            var contentChat = new ContentChat()
            {
                ContentText = contentChatViewModel.ContentText,
                Type = contentChatViewModel.Type,
                EmojiId = contentChatViewModel.EmojiId,
                RoomId = contentChatViewModel.RoomId,
                UserId = contentChatViewModel.UserId,
                PathAudio = contentChatViewModel.PathAudio,
                PathFilde = contentChatViewModel.PathFilde,
                PathImage = contentChatViewModel.PathImage,
                PathVideo = contentChatViewModel.PathVideo,
                TimeChat = DateTime.Now
            };

            var ccfb = new ContentChatFB()
            {
                Id = contentChat.Id.ToString(),
                ContentText = contentChat.ContentText,
                Type = contentChat.Type,
                EmojiId = contentChat.EmojiId.ToString(),
                UserId = contentChat.UserId,
                PathAudio = contentChat.PathAudio,
                PathFilde = contentChat.PathFilde,
                PathImage = contentChat.PathImage,
                PathVideo = contentChat.PathVideo,
                TimeChat = DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss")
            };

            await firebaseClient.Child(ROOM)
                                .Child(contentChat.RoomId.ToString())
                                .Child(contentChat.Id.ToString())
                                .PutAsync(ccfb);

            context.ContentChats.Add(contentChat);
            await context.SaveChangesAsync();
            return Ok();
        }
        #endregion
    }
}