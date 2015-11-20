using Portal.Models;
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
        
        [HttpGet]
        [Route("{id:int}/edit")]
        [Authorize(Roles = "editor, admin")]
        public ActionResult Edit(int id)
        {
            var course = db.Course.Where(p => id == p.ID).FirstOrDefault();
            if (course == null)
            {
                return View("Error");
            };
            return View(course);
        }
        
        [HttpPost]
        [Route("{id:int}/edit")]
        [Authorize(Roles = "editor, admin")]
        public ActionResult Edit(Course course)
        {
            return View();
        }

        [HttpPost]
        [Route("")]
        public ActionResult Index(string SearchFor)
        {
            ViewBag.Title = "Courses";
            ViewBag.SearchValue = SearchFor;
            var CourseList = db.Course.Where(x => x.Name.ToUpper().IndexOf(SearchFor.ToUpper()) >= 0).Take(50).ToArray();
            return View(CourseList);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddLesson(string Name, string Description, string Links, int id)
        {
            Course course = db.Course.Where(p => id == p.ID).FirstOrDefault();
            Person author = db.Users.Where(p => User.Identity.Name == p.UserName).FirstOrDefault();
            if(!course.Teachers.Contains(author))
                return View("Error");
            var lesson = new Lesson() { Name = Name, Description = Description, Links = Links };
            course.Lessons.Add(lesson);
            db.SaveChanges();
            return RedirectToAction("Course", id);
        }
    }
}