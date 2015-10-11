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
    }
}