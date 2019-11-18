using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WebApi.Models;

namespace Entities.Models
{
    public class Emoji : BaseEntity
    {
        public string NameEmoji { get; set; }
        public string PathImage { get; set; }
        [JsonIgnore]
        public virtual ICollection<ContentChat> ContentChats { get; set; }
        [JsonIgnore]
        public virtual TypeEmoji TypeEmoji { get; set; }
        [ForeignKey("TypeEmoji")]
        public Guid IdType { get; set; }
    }
}
