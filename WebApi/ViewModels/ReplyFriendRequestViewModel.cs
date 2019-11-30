using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.ViewModels
{
    public class ReplyFriendRequestViewModel
    {
        public Guid IdFriendRequest { get; set; }
        public bool IsAccept { get; set; }
    }
}