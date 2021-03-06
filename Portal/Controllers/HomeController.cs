﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Portal.Models;

namespace Portal.Controllers
{
    [RoutePrefix( "" )]
    public class HomeController : Controller
    {
        Models.ApplicationDbContext db = new Models.ApplicationDbContext();
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            var ArticleList = db.Article.OrderByDescending( x => x.Date_of_Creation ).Take( 5 ).ToArray();
            return View( ArticleList );
        }

        [HttpPost]
        public ActionResult Index( string SearchFor )
        {
            if( SearchFor != "" ) {
                ViewBag.Title = "Home Page";
                ViewBag.SearchValue = SearchFor;
                var ArticleList = db.Article.Where( x => x.Name.ToUpper().IndexOf( SearchFor.ToUpper() ) >= 0 ).Take( 50 ).ToArray();
                return View( ArticleList );
            } else {
                ViewBag.Title = "Home Page";
                var ArticleList = db.Article.OrderByDescending( x => x.Date_of_Creation ).Take( 5 ).ToArray();
                return View( ArticleList );
            }
        }

    }

    public class ErrorController : Controller
    {
        Models.ApplicationDbContext db = new Models.ApplicationDbContext();

        [HttpGet]
        [Route( "Error" )]
        public ActionResult Error( TempDataDictionary data )
        {
            List<ModelError> errorList = TempData["errors"] as List<ModelError>;

            ViewBag.Title = "Something went wrong";
            return View( errorList );
        }

    }
}