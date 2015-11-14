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
using System.Net.Http;
using System.Net;

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

        [HttpPost]
        [Route("{search:int}")]
        public ActionResult Index(string SearchFor)
        {
            ViewBag.SearchValue = SearchFor;
            var ArticleList = db.Article.Where(x => x.Name.ToUpper().IndexOf(SearchFor.ToUpper()) >= 0).Take(50).ToArray();
            return View(ArticleList);
        }


        [Authorize]
        [HttpGet]
        [Route("{id:int}/edit")]
        public ActionResult Edit(int id)
        {
            Article article = db.Article.Where(p => id == p.ID).FirstOrDefault();
            if (article.Author.UserName != User.Identity.Name && !User.IsInRole("admin"))
                return HttpNotFound();
            if (article != null)
            {
                var Blogs = db.Blog.OrderBy(r => r.Name).ToList().Select(rr =>
                    new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name, Selected = article.Blogs.Contains(rr) }).ToList();
                ViewBag.Blogs = Blogs;
                return View(article);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [Authorize]
        [HttpPost]
        [Route("{id:int}/edit")]
        public ActionResult Edit(int id, string[] Blogs, Article articleEdit)
        {

            Article article = db.Article.Where(p => id == p.ID).FirstOrDefault();
            if (article != null)
            {
                if (article.Author.UserName == User.Identity.Name || User.IsInRole("admin"))
                {
                    article.Text = articleEdit.Text;
                    article.Name = articleEdit.Name;
                    article.Blogs.Clear();
                    article.Blogs = db.Blog.Where(p => Blogs.Contains(p.Name)).ToList();
                    //db.Entry(article).State = EntityState.Modified;

                    db.SaveChanges();
                    return RedirectToAction("Index", "articles", article.ID);
                }
                else
                {
                    return HttpNotFound();
                }
            }
            else
            {
                return HttpNotFound();
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
        public ActionResult Create(string[] Blogs, Article articleEdit)
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
            /* if (!ModelState.IsValid)
             {
                 var errors = ModelState.SelectMany(x => x.Value.Errors.Select(z => z.Exception));
                 return View("Error " + errors);
             };*/
            var article = db.Article.Add(articleEdit);
            db.SaveChanges();
            if (article == null)
                return View("Error");
            return RedirectToAction("Index", "articles", new { id = article.ID });
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

        [Authorize]
        [HttpPost]
        [Route("{id:int}/like")]
        public JsonResult LikeArticle(HttpRequestMessage request, int id)
        {
            Article article = db.Article.Include(a => a.Author).Where(p => p.ID == id).FirstOrDefault();
            var um = new UserManager<Person>(new UserStore<Person>(db));
            var usr = um.FindByName(User.Identity.Name);
            int? likesCount = article.Likes_Count;
            if (likesCount == null)
            {
                likesCount = 0;
            }
            if (!article.Dislikes_Authors.Contains(usr))
            {
                if (article.Likes_Authors.Contains(usr))
                {
                    --likesCount;
                    article.Likes_Authors.Remove(usr);
                }
                else
                {
                    ++likesCount;
                    article.Likes_Authors.Add(usr);
                }
                article.Likes_Count = likesCount;
                db.SaveChanges();
            };
            return Json(new { likesCount = likesCount });
        }

        [Authorize]
        [HttpPost]
        [Route("{id:int}/dislike")]
        public JsonResult DislikeArticle(HttpRequestMessage request, int id)
        {
            Article article = db.Article.Include(a => a.Author).Where(p => p.ID == id).FirstOrDefault();
            var um = new UserManager<Person>(new UserStore<Person>(db));
            var usr = um.FindByName(User.Identity.Name);
            int? dislikesCount = article.Dislikes_Count;
            if (dislikesCount == null)
            {
                dislikesCount = 0;
            }
            if (!article.Likes_Authors.Contains(usr))
            {
                if (article.Dislikes_Authors.Contains(usr))
                {
                    --dislikesCount;
                    article.Dislikes_Authors.Remove(usr);
                }
                else
                {
                    ++dislikesCount;
                    article.Dislikes_Authors.Add(usr);
                }
                article.Dislikes_Count = dislikesCount;
                db.SaveChanges();
            };
            return Json(new { dislikesCount = dislikesCount });
        }
    }
}