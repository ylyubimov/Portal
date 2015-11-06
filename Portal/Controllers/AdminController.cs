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
        [HttpGet]
        public ActionResult AdminTable()
        {
            Person[] allPersons = db.Person.ToArray();
            ViewBag.Title = "People";
            return View(allPersons);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            Person person = db.Person.Where(p => id == p.Id).FirstOrDefault();

            person.Exists = false;

            //db.SaveChanges();

            return RedirectToAction("AdminTable");
        }

        [HttpPost]
        public ActionResult Change(string id)
        {
            Person person = db.Person.Where(p => id == p.Id).FirstOrDefault();
            /*
            if (person is Student)
            {
                Teacher p = new Teacher() { };

                p.First_Name = person.First_Name;
                p.Second_Name = person.Second_Name;
                p.Middle_Name = person.Middle_Name;
                p.Phone = person.Phone;
                p.Registration_Date = person.Registration_Date;
                p.Last_Date_Was_Online = person.Last_Date_Was_Online;
                p.Email = person.Email;
                p.Picture = person.Picture;
                p.Subscribed_Courses = person.Subscribed_Courses;
                p.Written_Articles = person.Written_Articles;
                p.Blogs = person.Blogs;

                db.Person.Remove((Student)person);

                db.Person.Add(p);

                ////db.SaveChanges();
            }
            else
            {
                Student p = new Student() { };

                p.First_Name = person.First_Name;
                p.Second_Name = person.Second_Name;
                p.Middle_Name = person.Middle_Name;
                p.Phone = person.Phone;
                p.Registration_Date = person.Registration_Date;
                p.Last_Date_Was_Online = person.Last_Date_Was_Online;
                p.Email = person.Email;
                p.Picture = person.Picture;
                p.Subscribed_Courses = person.Subscribed_Courses;
                p.Written_Articles = person.Written_Articles;
                p.Blogs = person.Blogs;

                db.Person.Remove(person);

                db.Person.Add(p);

                //db.SaveChanges();
            }
            */
            return RedirectToAction("AdminTable");
        }
    }
}