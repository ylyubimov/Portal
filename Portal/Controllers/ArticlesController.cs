using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Models;

namespace Portal.Controllers
{
    [RoutePrefix("articles")]
    public class ArticlesController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Articles

        [Authorize]
        [Route("{id:int}")]
        public ActionResult Index(int id)
        {
            Article article = db.Article.Where(p => id == p.ID).FirstOrDefault();
            if (article == null)
            {
                return HttpNotFound();
            }
             
            return View(article);
        }


        [Authorize]
        [HttpGet]
        [Route("{id:int}/edit")]
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
        [Route("{id:int}/edit")]
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

        [Authorize]
        [HttpPost]
        public ActionResult AddComment(string comment, int id)
        {
            Article article = db.Article.Where(p => id == p.ID).FirstOrDefault();

            Person authorComment = db.Person.Where(p => User.Identity.Name == p.UserName).FirstOrDefault();
            //TODO: delete this line

            Comment c = new Comment();
            
            c.Text = comment;
            c.Article = article;
            c.Author = authorComment;
            c.Create_Time = DateTime.Now;

            article.Comments.Add(c);

            db.SaveChanges();                        
            return RedirectToAction("index", id);
        }
    }
}