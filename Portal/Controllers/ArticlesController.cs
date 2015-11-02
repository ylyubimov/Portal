using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;

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
            if(article.Author.UserName != User.Identity.Name)
                return View("You are not the author of this article");
            if (article != null)
            {
                var Blogs = db.Blog.OrderBy(r => r.Name).ToList().Select(rr =>
                    new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name, Selected = article.Blogs.Contains(rr) }).ToList();
                ViewBag.Blogs = Blogs;
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
        public ActionResult Edit(int id, string[] Blogs,Article articleEdit)
        {
            if (!ModelState.IsValid)
                return View("Error");
            Article article = db.Article.Where(p => id == p.ID).FirstOrDefault();
            if (article != null)
            {
                if(article.Author.UserName == User.Identity.Name)
                {
                    article.Text = articleEdit.Text;
                    article.Name = articleEdit.Name;
                    article.Blogs = db.Blog.Where(p => Blogs.Contains(p.Name) ).ToArray();
                    db.Entry(article).State = EntityState.Modified;

                    db.SaveChanges();
                    return RedirectToAction( "Index", "articles", article.ID);
                }
                else
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
        [HttpGet]
        [Route("Create")]
        public ActionResult Create()
        {
            var Blogs = db.Blog.OrderBy(r => r.Name).ToList().Select(rr =>
                      new SelectListItem { Value = rr.Name, Text = rr.Name, Selected = false }).ToList();
            ViewBag.Blogs = Blogs;
            return View(new Article() { Name = "Name", Text = "Text" });           
        }

        [Authorize]
        [HttpPost]
        [Route("Create")]
        public ActionResult Create( string[] Blogs, Article articleEdit)
        {
            articleEdit.Blogs = db.Blog.Where(p => Blogs.Contains(p.Name)).ToArray();
            var um = new UserManager<Person>(new UserStore<Person>(db));
            var author = um.FindByName(User.Identity.Name);
            if (author == null)
                return View("Can't find your account in persons");
            articleEdit.Author = author;
            articleEdit.Date_of_Creation = DateTime.Now;
            articleEdit.Likes_Count = 0;
            articleEdit.Dislikes_Count = 0;
            if (!ModelState.IsValid)
            {
                var errors = ModelState.SelectMany(x => x.Value.Errors.Select(z => z.Exception));
                return View("Error " + errors);
            };
            db.Article.Add(articleEdit);
            db.SaveChanges();
            var article = db.Article.Where(m => m.Date_of_Creation == articleEdit.Date_of_Creation).FirstOrDefault();
            if (article == null)
                return View("Error");
            return RedirectToAction("Index", "articles", article.ID);
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