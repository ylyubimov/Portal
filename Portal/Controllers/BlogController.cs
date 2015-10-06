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
    }
}