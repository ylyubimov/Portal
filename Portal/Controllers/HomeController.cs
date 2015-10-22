﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Models;
namespace Portal.Controllers
{
    public class HomeController : Controller
    {
        Models.ApplicationDbContext db = new Models.ApplicationDbContext();
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            var ArticleList = db.Article.OrderBy(x => x.Date_of_Creation ).Take(5).ToArray();
            return View(ArticleList);
        }
            
        [HttpPost]
        public ActionResult Index(string SearchFor)
        {
            ViewBag.Title = "Search for "+ SearchFor;
            var ArticleList = db.Article.Where(x => x.Name.ToUpper().IndexOf(SearchFor.ToUpper()) >= 0).Take(50).ToArray();
            return View(ArticleList);
        }

    }
}