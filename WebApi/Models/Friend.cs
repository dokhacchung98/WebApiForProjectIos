using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WebApi.Models;

namespace Entities.Models
{
    public class Friend : BaseEntity
    {
        public virtual ApplicationUser User1 { get; set; }
        public virtual ApplicationUser User2 { get; set; }

        public DateTime? LastInterractive { get; set; }

        [ForeignKey("User1")]
        public string User1Id { get; set; }

        [ForeignKey("User2")]
        public string User2Id { get; set; }
    }
}