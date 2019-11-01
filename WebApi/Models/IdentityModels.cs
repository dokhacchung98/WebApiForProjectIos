using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Entities.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;

namespace WebApi.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string Avatar { get; set; }

        public string Wallpaper { get; set; }

        public string Address { get; set; }

        public string FullName { get; set; }

        public DateTime? DoB { get; set; }

        public string Phone { get; set; }
        [JsonIgnore]
        public virtual ICollection<ContentChat> ContentChats { get; set; }
        [JsonIgnore]
        public virtual ICollection<UserJoinRoom> UserJoinRooms { get; set; }
        [JsonIgnore]
        public virtual ICollection<Friend> Friends { get; set; }
        [JsonIgnore]
        public virtual ICollection<BlockUser> Blocks { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
        // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
        var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }
}