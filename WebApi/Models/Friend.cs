using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WebApi.Models;

namespace Entities.Models
{
    public class Friend : BaseEntity
    {
        //[JsonIgnore]
        public virtual ApplicationUser User1 { get; set; }
        //[JsonIgnore]
        public virtual ApplicationUser User2 { get; set; }

        [ForeignKey("User1")]
        public string User1Id { get; set; }

        [ForeignKey("User2")]
        public string User2Id { get; set; }
    }
}