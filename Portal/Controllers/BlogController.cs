using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Models;

namespace Portal.Controllers
{
    public class BlogsController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Blog
        public ActionResult Index()
        {
            ViewBag.Title = "Blogs";
            Blog[] blogs = db.Blog.Where(p => true).ToArray();
            return View(blogs);
        }

        public ActionResult Blog(int? id)
        {
            if (id != null)
            {
                Blog blog = db.Blog.Where(p => id == p.ID).FirstOrDefault();
                if (blog != null)
                {
                    return View(blog);
                } else
                {
                    return View("Error");
                }
            }
            else
            {
                return View("Index");
            }
        }

        [HttpPost]
        public ActionResult Index(string SearchFor)
        {
            ViewBag.Title = "Search for " + SearchFor;
            var BlogList = db.Blog.Where(x => x.Name.ToUpper().IndexOf(SearchFor.ToUpper()) >= 0).Take(50).ToArray();
            return View(BlogList);
        }

    }
}