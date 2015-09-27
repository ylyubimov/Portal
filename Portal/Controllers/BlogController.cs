using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    // TODO вынести в Models
    public class Blog
    {
        public int id;
        public Article[] articles;

        public Blog(int _id)
        {
            id = _id;

            // TODO временная заглушка
            articles = new Article[5];
            for (int i = 0; i < 5; ++i)
            {
                articles[i] = new Article(i * id);
            }
        }
    }

    public class BlogsController : Controller
    {
        private Blog[] getBlogs()
        {
            // TODO временная заглушка
            Blog[] blogs = new Blog[7];
            for (int i = 0; i < 7; ++i)
            {
                blogs[i] = new Blog(i);
            }

            return blogs;
        }
        // GET: Blog
        public ActionResult Index()
        {
            return View(getBlogs());
        }

        public ActionResult Blog(int? id)
        {
            if (id != null)
            {
                return View(new Blog((int)id));
            }
            else
            {
                return View("Index");
            }
        }

    }
}