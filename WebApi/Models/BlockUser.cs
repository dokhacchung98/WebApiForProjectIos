using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WebApi.Models;

namespace Entities.Models
{
    public class BlockUser : BaseEntity
    {
        [ForeignKey("User")]
        public string UserID { get; set; }
        //[JsonIgnore]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("UserBlock")]
        public string UserBlockId { get; set; }
        //[JsonIgnore]
        public virtual ApplicationUser UserBlock { get; set; }
    }
}
