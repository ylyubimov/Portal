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
        public ActionResult Create()
        {
            return View(new Blog() { Name = "Name" });
        }

        [Authorize]
        [HttpPost]
        [Route("Create")]
        public ActionResult Create( Blog newBlog )
        {
            var um = new UserManager<Person>(new UserStore<Person>(db));
            var author = um.FindByName(User.Identity.Name);
            if (author != null)
                return View("Error");
            newBlog.Author = author;
            newBlog.Likes_Count = 0;
            newBlog.Dislikes_Count = 0;
            var blog = db.Blog.Add(newBlog);
            db.SaveChanges();
            if (blog == null)
                return View("Error");
            return RedirectToAction("Blog", "Blogs", new { id = blog.ID });
        }
    }
}