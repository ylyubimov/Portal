using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Portal.Controllers
{
    public class MyViewModel
    {
        public Course[] courses { get; set; }
        public HashSet<int> checkedIds { get; set; }
        public string personId { get; set; }
    }

    [RoutePrefix("admin")]
    public class AdminController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Articles
        [Route("")]
        [HttpGet]
        public ActionResult AdminTable()
        {
            Person[] allPersons = db.Users.Where(p => true == p.Exists).ToArray();
            ViewBag.Title = "People";
            return View(allPersons);
        }

        [HttpPost]
        public ActionResult Save(string id , string[] checkedCourses)
        {
            Person person = db.Users.Where(p => id == p.Id).FirstOrDefault();
            person.Subscribed_Courses.Clear();
            if (checkedCourses != null)
            {
                foreach (string name in checkedCourses)
                {
                    person.Subscribed_Courses.Add(db.Course.Where(c => name == c.Name).FirstOrDefault());
                }
            }
            db.SaveChanges();
            return RedirectToAction("AdminTable");
        }

        [Route("{id}")]
        [HttpGet]
        public ActionResult Index(string id)
        {
            Course[] allCourses = db.Course.ToArray();
            Person person = db.Users.Where(p => id == p.Id).FirstOrDefault();
            Course[] studentCourses = person.Subscribed_Courses.ToArray();
            HashSet<int> idCoursesSet = new HashSet<int>();
            if (studentCourses != null)
            {
                foreach (Course c in studentCourses)
                {
                    idCoursesSet.Add(c.ID);
                }
            }
            var viewModel = new MyViewModel
            {
                courses = allCourses,
                checkedIds = idCoursesSet,
                personId = id
            };
            
            return View(viewModel);
        }

        
        [HttpPost]
        public ActionResult Delete(string id)
        {
            //Person person = db.Users.Where(p => id == p.Id).FirstOrDefault();
            //person.Exists = false;
            Person.DeleteUser(db, id);
            db.SaveChanges();
            return RedirectToAction("AdminTable");
        }

        [HttpPost]
        public ActionResult ChangeType(string id)
        {
            Person person = db.Users.Where(p => id == p.Id).FirstOrDefault();

            if (person.Person_Type == "Student")
            {
                person.Person_Type = "Teacher";
            } else
            {
                person.Person_Type = "Student";
            }
            db.SaveChanges();
            return RedirectToAction("AdminTable");
        }
        [HttpPost]
        public ActionResult ChangeRole(string id)
        {
            Person p = db.Users.Where(pp => id == pp.Id).FirstOrDefault();

            var userManager = new ApplicationUserManager(new UserStore<Person>(db));

            if (userManager.IsInRole(p.Id, "editor"))
            {
                userManager.RemoveFromRole(p.Id, "editor");
                userManager.AddToRole(p.Id, "admin");
            } else if (userManager.IsInRole(p.Id, "admin"))
            {
                userManager.RemoveFromRole(p.Id, "admin");
                userManager.AddToRole(p.Id, "user");

            } else
            {
                userManager.RemoveFromRole(p.Id, "user");
                userManager.AddToRole(p.Id, "editor");
            }

            db.SaveChanges();
                       
            return RedirectToAction("AdminTable");
        }
    }
}