using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.ModelsFirebase
{
    public class FriendRequestFB
    {
        public string ID { get; set; }
        public string IDUserSend { get; set; }
        public string IDUser { get; set; }
        public string Content { get; set; }
        public string TimeSend { get; set; }
    }
}