using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Models;

namespace Portal.Controllers
{
    public class ArticlesController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Articles
        public ActionResult Index(int? id)
        {
            if (id != null)
            {
                Article article = db.Article.Where(p => id == p.ID).FirstOrDefault();
                if (article != null)
                {
                    return View(article);
                } else
                {
                    return View("Error");
                }
            }
            else
            {
                return View("Error");
            }
        }
    }
}