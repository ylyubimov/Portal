using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Models;

namespace Portal.Controllers
{
    [RoutePrefix("persons")]
    public class PersonsController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        private Person[] getTeachers(Person[] persons)
        {
            return persons.Where(p => p is Teacher).ToArray();
        }

        private Person[] getStudents(Person[] persons)
        {
            return persons.Where(p => p is Student).ToArray();
        }

        // GET: Articles
        [HttpGet]
        [Route("")]
        public ActionResult Index(string view, string type)
        {
            ViewBag.Title = "People";
            ViewBag.ExtendType = "person";
            Person[] AllPersons = db.Person.ToArray();
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
                Person person = db.Person.Where(p => id == p.Id).FirstOrDefault();
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
            ViewBag.Title = "Search for " + SearchFor;
            var PersonList = db.Person.Where(x => (x.First_Name + " " + x.Second_Name).ToUpper().IndexOf(SearchFor.ToUpper()) >= 0 ||
                                                   (x.First_Name + " "+ x.Middle_Name + " " + x.Second_Name).ToUpper().IndexOf(SearchFor.ToUpper()) >= 0
                                                   ).Take(50).ToArray();
            Person[][] persons = new Person[][] { getTeachers(PersonList), getStudents(PersonList) };
            return View("IndexGrid", persons);
        }
    }
}