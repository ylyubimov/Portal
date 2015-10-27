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

        // GET: Articles
        [Route("")]
        public ActionResult Index()
        {
            ViewBag.Title = "Person";
            Person[] persons = db.Person.ToArray();
            return View(persons);
        }

        [Route("{id:int}")]
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
            return View(PersonList);
        }
    }
}