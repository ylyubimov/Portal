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
    [RoutePrefix("persons")]
    public class PersonsController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        private Person[] getTeachers(Person[] persons)
        {
            return persons.Where(p => p.Person_Type == "Teacher").ToArray();
        }

        private Person[] getStudents(Person[] persons)
        {
            return persons.Where(p => p.Person_Type == "Student").ToArray();
        }

        // GET: Articles
        [HttpGet]
        [Route("")]
        public ActionResult Index(string view, string type)
        {
            var list = db.Roles.OrderBy(r => r.Name).ToList().Select(rr =>  new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;
            ViewBag.Title = "People";
            ViewBag.ExtendType = "person";
            Person[] AllPersons = db.Users.ToArray();
            Person[][] persons = new Person[][] { getTeachers(AllPersons), getStudents(AllPersons) };
            ViewBag.Type = type;
            
            if (view != null && view == "grid")
            {
                ViewBag.View = "grid";
            }
            return View(persons);
        }

        [Route("{id}")]
        public ActionResult Person(string id)
        {
            if (id  != "")
            {
                Person person = db.Users.Where(p => id == p.Id).FirstOrDefault();
                if (person != null)
                {
                    return View(person);
                }
                else
                {
                    return HttpNotFound();
                }
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        [Route("")]
        public ActionResult Index(string SearchFor)
        {
            if (SearchFor != "")
            {
                ViewBag.Title = "People";
                ViewBag.SearchValue = SearchFor;
                var PersonList = db.Users.Where(x => (x.First_Name + " " + x.Second_Name).ToUpper().IndexOf(SearchFor.ToUpper()) >= 0 ||
                                                       (x.First_Name + " " + x.Middle_Name + " " + x.Second_Name).ToUpper().IndexOf(SearchFor.ToUpper()) >= 0
                                                       ).Take(50).ToArray();
                Person[][] persons = new Person[][] { getTeachers(PersonList), getStudents(PersonList) };
                return View(persons);
            }
            else
            {
                var list = db.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
                ViewBag.Roles = list;
                ViewBag.Title = "People";
                ViewBag.ExtendType = "person";
                Person[] AllPersons = db.Users.ToArray();
                Person[][] persons = new Person[][] { getTeachers(AllPersons), getStudents(AllPersons) };
                return View(persons);
            }
        }
      
    }
}