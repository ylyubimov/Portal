using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Models;

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
            ViewBag.Title = "Search for " + SearchFor;
            var BlogList = db.Blog.Where(x => x.Name.ToUpper().IndexOf(SearchFor.ToUpper()) >= 0).Take(50).ToArray();
            return View(BlogList);
        }
    }
}