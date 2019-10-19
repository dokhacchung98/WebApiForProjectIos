using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Models;

namespace Entities.Models
{
    public class Room : BaseEntity
    {
        public string ColorRoom { get; set; }
        public string StickerRoom { get; set; }
        public string KeyVideoCall { get; set; }
        public string KeyCall { get; set; }
        public string NameRoom { get; set; }
        public string PathAvatar { get; set; }

        public virtual ICollection<UserJoinRoom> UserJoinRooms { get; set; }
        public virtual ICollection<ContentChat> ContentChats { get; set; }
    }
}