using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.ViewModels
{
    public class AddRoomViewModel
    {
        public string NameRoom { get; set; }
        public string UserCreate { get; set; }
        public IList<string> ListUserId { get; set; }
    }
}