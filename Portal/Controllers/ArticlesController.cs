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

        [Authorize]
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


        [Authorize]
        public ActionResult Edit(int id)
        {
            Article article = db.Article.Where(p => id == p.ID).FirstOrDefault();
            if (article != null)
            {
                return View(article);
            }
            else
            {
                return View("Error");
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(ArticleEdit articleEdit)
        {
            if (articleEdit.ID != null)
            {
                Article article = db.Article.Where(p => articleEdit.ID == p.ID).FirstOrDefault();
                if (article != null)
                {
                    article.Text = articleEdit.Text;
                    article.Name = articleEdit.Text;
                    db.Article.Add(article);
                    db.SaveChanges();
                    return View(article);
                }
                else
                {
                    return View("Error");
                }
            }
            else
            {
                Article article = new Article() { Text = articleEdit.Text, Name = articleEdit.Name, Date_of_Creation = DateTime.Now, Author = db.Person.Where( u => u.UserName == User.Identity.Name).FirstOrDefault()  };
                db.Article.Add(article);
                db.SaveChanges();
                return View(article);
            }
        }
    }
}