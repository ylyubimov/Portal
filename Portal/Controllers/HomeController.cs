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
            /* пока нет базы данных, потом надо будет брать из бд*/
            Portal.Models.Article[] ArticleList = new Portal.Models.Article[2] { new Portal.Models.Article(),new Portal.Models.Article() };
            ArticleList[0].Name = "Title1";
            ArticleList[0].Text = "Article1 Text;Article1 Text;Article1 Text;Article1 Text;Article1 Text;Article1 Text;Article1 Text;Article1 Text;Article1 Text;Article1 Text;Article1 Text;Article1 Text;Article1 Text;Article1 Text;Article1 Text;Article1 Text;Article1 Text;Article1 Text;Article1 Text;Article1 Text;Article1 Text;Article1 Text;Article1 Text;Article1 Text;Article1 Text;Article1 Text;Article1 Text;Article1 Text;Article1 Text;Article1 Text;";
            ArticleList[1].Name = "Title2";
            ArticleList[1].Text = "Article2 Text;Article2 Text;Article2 Text;Article2 Text;Article2 Text;Article2 Text;Article2 Text;Article2 Text;Article2 Text;Article2 Text;Article2 Text;Article2 Text;Article2 Text;Article2 Text;Article2 Text;Article2 Text;Article2 Text;Article2 Text;Article2 Text;Article2 Text;Article2 Text;Article2 Text;Article2 Text;Article2 Text;Article2 Text;Article2 Text;Article2 Text;Article2 Text;Article2 Text;Article2 Text;Article2 Text;Article2 Text;Article2 Text;";
            /*****************************************************/
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