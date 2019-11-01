using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WebApi.Models;

namespace Entities.Models
{
    public class UserJoinRoom : BaseEntity
    {
        public string NickName { get; set; }
        public DateTime? LastInterractive { get; set; }
        //[JsonIgnore]
        public virtual Room Room { get; set; }
        [ForeignKey("Room")]
        public Guid RoomId { get; set; }
        //[JsonIgnore]
        public virtual ApplicationUser User { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
    }
}
