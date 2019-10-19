using Entities.Models;
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
        public string ContentText { get; set; }
        public int Type { get; set; }
        public string PathImage { get; set; }
        public string PathVideo { get; set; }
        public string PathFilde { get; set; }
        public string PathAudio { get; set; }
        public DateTime TimeChat { get; set; }

        [ForeignKey("Emoji")]
        public Guid? EmojiId { get; set; }
        public virtual Emoji Emoji { get; set; }

        [ForeignKey("Room")]
        public Guid RoomId { get; set; }
        public virtual Room Room { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}