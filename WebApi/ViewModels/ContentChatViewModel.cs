using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.ViewModels
{
    public class ContentChatViewModel
    {
        public string ContentText { get; set; }
        public int Type { get; set; }
        public string PathImage { get; set; }
        public string PathVideo { get; set; }
        public string PathFilde { get; set; }
        public string PathAudio { get; set; }
        public Guid? EmojiId { get; set; }
        public Guid RoomId { get; set; }
    }
}