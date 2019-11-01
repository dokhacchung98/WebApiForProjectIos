using Entities.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using WebApi.Models;

namespace Entities.Models
{
    public class ContentChat : BaseEntity
    {
        public DateTime TimeChat { get; set; }

        [ForeignKey("Emoji")]
        public Guid? EmojiId { get; set; }
        //[JsonIgnore]
        public virtual Emoji Emoji { get; set; }

        [ForeignKey("Room")]
        public Guid RoomId { get; set; }
        //[JsonIgnore]
        public virtual Room Room { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        //[JsonIgnore]
        public virtual ApplicationUser User { get; set; }
    }
}