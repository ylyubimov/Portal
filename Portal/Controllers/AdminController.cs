using System;
using System.Collections;
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

        [Authorize(Roles = "admin")]
        [Route("")]
        [HttpGet]
        public ActionResult AdminTable()
        {
            Person[] allPersons = db.Users.Where(p => true == p.Exists).ToArray();
            Array.Sort(allPersons, new Comparison<Person>((x, y) => String.Compare(x.Second_Name, y.Second_Name)));
            ViewBag.Title = "Люди";
            return View(allPersons);
        }

        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "admin")]
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


        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Delete(string id)
        {
            Person person = db.Users.Where(p => id == p.Id).FirstOrDefault();
            var userManager = new ApplicationUserManager(new UserStore<Person>(db));
            if (User.Identity.GetUserName() == person.UserName)
            {
                return RedirectToAction("AdminTable");
            }
            if (userManager.IsInRole(person.Id, "admin"))
            {
                var countAdmin = 0;
                var allPersons = db.Users.Where(pp => true == pp.Exists).ToArray();
                foreach (var p in allPersons)
                {
                    if (userManager.IsInRole(p.Id, "admin"))
                    {
                        countAdmin += 1;
                    }
                }
                if (countAdmin == 1)
                {
                    return RedirectToAction("AdminTable");
                }
            }
            Person.DeleteUser(db, id);
            db.SaveChanges();
            return RedirectToAction("AdminTable");
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult ChangeType(string id)
        {
            Person person = db.Users.Where(p => id == p.Id).FirstOrDefault();

            if (person.Person_Type == "Teacher")
            {
                person.Person_Type = "Admin";
            }
            else if (person.Person_Type == "Admin")
            {
                person.Person_Type = "Student";
            }
            else
            {
                person.Person_Type = "Teacher";
            }
            db.SaveChanges();
            return RedirectToAction("AdminTable");
        }
        [Authorize(Roles = "admin")]
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
                var countAdmin = 0;
                var allPersons = db.Users.Where(pp => true == pp.Exists).ToArray();
                foreach (var person in allPersons){
                    if (userManager.IsInRole(person.Id, "admin"))
                    {
                        countAdmin += 1;
                    }

                }
                if (countAdmin == 1)
                {
                    return RedirectToAction("AdminTable");
                } else
                {
                    userManager.RemoveFromRole(p.Id, "admin");
                    userManager.AddToRole(p.Id, "user");
                }                
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