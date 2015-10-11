using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Models;
namespace Portal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var db = new Models.ApplicationDbContext();
            var ArticleList = db.Article.OrderBy(x => x.Date_of_creation).Take(5).ToArray();
            return View(ArticleList);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}