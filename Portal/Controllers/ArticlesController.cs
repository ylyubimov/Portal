using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    // TODO вынести в Models
    public class Article
    {
        public int id = 0;
        public String author = "author";
        public String text = "text";

        public Article(int _id)
        {
            id = _id;
        }
    }
    public class ArticlesController : Controller
    {
        // GET: Articles
        public ActionResult Index(int? id)
        {
            if (id != null)
            {
                return View(new Article((int)id));
            }
            else
            {
                return View(new Article(-1));
            }
        }
    }
}