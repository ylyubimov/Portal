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
    [RoutePrefix("admin")]
    public class AdminController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Articles
        [Route("")]
        [HttpGet]
        public ActionResult AdminTable()
        {
            Person[] allPersons = db.Person.Where(p => true == p.Exists).ToArray();
            ViewBag.Title = "People";
            return View(allPersons);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            Person person = db.Person.Where(p => id == p.Id).FirstOrDefault();
            person.Exists = false;
            db.SaveChanges();
            return RedirectToAction("AdminTable");
        }

        [HttpPost]
        public ActionResult Change(string id)
        {
            Person person = db.Person.Where(p => id == p.Id).FirstOrDefault();
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
    }
}