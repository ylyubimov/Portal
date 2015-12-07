using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Portal.Controllers
{
    [RoutePrefix("blog")]
    public class BlogsController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Blog
        [Route("")]
        public ActionResult Index()
        {
            ViewBag.Title = "Blogs";
            ViewBag.Action = "/blog";
            Blog[] blogs = db.Blog.Where(p => true).ToArray();
            return View(blogs);
        }

        [Route("{id:int}")]
        public ActionResult Blog(int id)
        {
            Blog blog = db.Blog.Where(p => id == p.ID).FirstOrDefault();
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        [HttpPost]
        [Route("")]
        public ActionResult Index(string SearchFor)
        {
            ViewBag.Title = "Blogs";
            ViewBag.SearchValue = SearchFor;
            var BlogList = db.Blog.Where(x => x.Name.ToUpper().IndexOf(SearchFor.ToUpper()) >= 0).Take(50).ToArray();
            return View(BlogList);
        }

        [HttpGet]
        [Route("Create")]
        [Authorize(Roles = "editor, admin")]
        public ActionResult Create()
        {
            return View(new Blog() { Name = "Name" });
        }
        
        [HttpPost]
        [Route("Create")]
        [Authorize(Roles = "editor, admin")]
        public ActionResult Create( Blog newBlog )
        {
            var um = new UserManager<Person>(new UserStore<Person>(db));
            var author = um.FindByName(User.Identity.Name);
            if (author == null)
                return View("Error");
            newBlog.Author = author;
            newBlog.Likes_Count = 0;
            newBlog.Dislikes_Count = 0;
            var blog = db.Blog.Add(newBlog);

            //Какой-то неопознанный баг, пришлось костылить, простите меня(
            ModelState["Author"].Errors.Clear();

            if (!ModelState.IsValid)
            {
                return View(blog);
            }

            db.SaveChanges();
            if (blog == null)
                return View("Error");
            return RedirectToAction("Blog", "Blogs", new { id = blog.ID });
        }

        [HttpGet]
        [Route("{id:int}/edit")]
        [Authorize(Roles = "editor, admin")]
        public ActionResult Edit( int id )
        {
            Blog blog = db.Blog.Where(b => b.ID == id).FirstOrDefault();
            if (blog == null)
            {
                HttpNotFound();
            }
            return View(blog);
        }

        [HttpPost]
        [Route("{blogId:int}/edit")]
        [Authorize(Roles = "editor, admin")]
        public ActionResult Edit(int blogId, Blog editedBlog)
        {
            Blog blog = db.Blog.Where(b => b.ID == blogId).FirstOrDefault();
            if (blog != null)
            {
                if (blog.Author.UserName == User.Identity.Name || User.IsInRole("admin"))
                {
                    blog.Name = editedBlog.Name;

                    //Какой-то неопознанный баг, пришлось костылить, простите меня(
                    ModelState["Author"].Errors.Clear();

                    if (!ModelState.IsValid)
                    {
                        return View(blog);
                    }

                    db.SaveChanges();
                }
            }
            return RedirectToAction("Blog", "Blogs", new { id = blogId });
        }

        [HttpGet]
        [Route("{blogId:int}/delete")]
        [Authorize(Roles = "editor, admin")]
        public ActionResult Delete(int blogId)
        {
            Blog blog = db.Blog.Where(b => b.ID == blogId).First();

            if (blog == null)
            {
                return HttpNotFound();
            }
            if (blog.Author.UserName != User.Identity.Name && !User.IsInRole("admin"))
            {
                return View("Error", "У вас нет прав на редактирование этих материалов");
            }

            Article[] articles = blog.Articles.ToArray();
            foreach (Article article in articles)
            {
                article.Blogs.Remove(blog);
                if (article.Blogs.ToArray().Length == 0)
                {
                    Person author = article.Author;
                    author.Written_Articles.Remove(article);

                    Comment[] comments = db.Comment.Where(p => p.Article.ID == article.ID).ToArray();
                    db.Article.Remove(article);
                    foreach (Comment comment in comments)
                    {
                        db.Comment.Remove(comment);
                    }
                }
            }

            Course[] courses = db.Course.Where(c => true).ToArray();
            foreach (Course course in courses)
            {
                if (course.Blogs.Contains(blog))
                {
                    course.Blogs.Remove(blog);
                }
            }
            db.Blog.Remove(blog);

            db.SaveChanges();
            return RedirectToAction("index");
        }
    }
}