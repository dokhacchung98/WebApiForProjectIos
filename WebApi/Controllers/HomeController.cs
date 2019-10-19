using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApi.MyDBContext;

namespace WebApi.Controllers
{
    public class HomeController : Controller
    {
        private MyDbContext context;
        public HomeController()
        {
            context = MyDbContext.Create();
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            var listEmoji = context.Emoji.ToList();
            var listTypeEmoji = context.TypeEmojis.ToList();
            ViewBag.LIST = listEmoji;
            ViewBag.LISTTYPE = listTypeEmoji;
            return View();
        }
    }
}
