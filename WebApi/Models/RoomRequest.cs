using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WebApi.Models;

namespace Entities.Models
{
    public class RoomRequest : BaseEntity
    {
        public string Content { get; set; }
        public Guid RoomId { get; set; }
        //[JsonIgnore]
        public ApplicationUser UserSend { get; set; }
        [ForeignKey("UserSend")]
        public string UserSendID { get; set; }
        //[JsonIgnore]
        public ApplicationUser User { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
    }
}
