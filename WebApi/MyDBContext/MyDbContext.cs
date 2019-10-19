using Entities.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApi.Models;

namespace WebApi.MyDBContext
{
    public class MyDbContext : IdentityDbContext<ApplicationUser>
    {
        public MyDbContext() : base("MyDbContext", throwIfV1Schema: false)
        {
        }

        public MyDbContext(string connectString) : base(connectString)
        {
        }

        public static MyDbContext Create()
        {
            return new MyDbContext();
        }

        public DbSet<Emoji> Emoji { get; set; }
        public DbSet<TypeEmoji> TypeEmojis { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<BlockUser> BlockUsers { get; set; }
        public DbSet<ContentChat> ContentChats { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<RoomRequest> RoomRequests{ get; set; }
        public DbSet<UserJoinRoom> UserJoinRooms{ get; set; }


    }
}