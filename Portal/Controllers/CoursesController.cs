﻿using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    [RoutePrefix("course")]
    public class CoursesController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        //
        // GET: /Courses/
        [Route("")]
        public ActionResult Index()
        {
            ViewBag.Title = "Courses";
            ViewBag.Action = "/course";
            var courses = db.Course.OrderBy(t => t.Report_Date).ToArray();
            return View(courses);
        }

        [Route("{id:int}")]
        public ActionResult Course(int id)
        {
            Course course = db.Course.Where(p => id == p.ID).FirstOrDefault();
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        [Authorize]
        [HttpGet]
        [Route("{id:int}/edit")]
        public ActionResult Edit(int id)
        {
            var course = db.Course.Where(p => id == p.ID).FirstOrDefault();
            if (course == null)
            {
                return View("Error");
            };
            return View(course);
        }

        [Authorize]
        [HttpPost]
        [Route("{id:int}/edit")]
        public ActionResult Edit(Course course)
        {
            return View();
        }

        [HttpPost]
        [Route("")]
        public ActionResult Index(string SearchFor)
        {
            ViewBag.Title = "Search for " + SearchFor;
            var CourseList = db.Course.Where(x => x.Name.ToUpper().IndexOf(SearchFor.ToUpper()) >= 0).Take(50).ToArray();
            return View(CourseList);
        }
    }
}