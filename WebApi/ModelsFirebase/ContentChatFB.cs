using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.ModelsFirebase
{
    public class ContentChatFB
    {
        public string Id { get; set; }
        public string ContentText { get; set; }
        public int Type { get; set; }
        public string PathImage { get; set; }
        public string PathVideo { get; set; }
        public string PathFilde { get; set; }
        public string PathAudio { get; set; }
        public string EmojiId { get; set; }
        public string UserId { get; set; }
        public string TimeChat { get; set; }
    }
}