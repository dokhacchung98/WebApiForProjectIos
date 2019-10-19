using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Entities.Models
{
    public class TypeEmoji : BaseEntity
    {
        public string NameType { get; set; }
        public string PathThumbnail { get; set; }
        public virtual ICollection<Emoji> Emojis { get; set; }
    }
}