using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class CoursesController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        //
        // GET: /Courses/
        public ActionResult Index()
        {
            ViewBag.Title = "Courses";
            var courses = db.Course.OrderBy(t => t.Report_Date).ToArray();
            return View(courses);
        }

        public ActionResult Course(int? id)
        {
            if (id != null)
            {
                Course course = db.Course.Where(p => id == p.ID).FirstOrDefault();
                if (course != null)
                {
                    return View(course);
                }
                else
                {
                    return View("Error");
                }
            }
            else
            {
                return View("Index");
            }
        }

        [Authorize]
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
        public ActionResult Edit(Course course)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string SearchFor)
        {
            ViewBag.Title = "Search for " + SearchFor;
            var CourseList = db.Course.Where(x => x.Name.ToUpper().IndexOf(SearchFor.ToUpper()) >= 0).Take(50).ToArray();
            return View(CourseList);
        }
    }
}