using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Models;

namespace Portal.Controllers
{
    public class PeopleController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        Person person1 = new Person
        {
            Person_Type = "Student",
            First_Name = "Ivan"

        };
        Person person2 = new Person
        {
            Person_Type = "Teacher",
            First_Name = "Petr"

        };
        Person person3 = new Person
        {
            Person_Type = "Student",
            First_Name = "Lena"

        };

        // GET: Articles
        public ActionResult Index()
        {
            db.Person.Add(person1);
            db.Person.Add(person2);
            db.Person.Add(person3);
            Person[] persons = db.Person.Where(p => true).ToArray();
            if (persons.Count() == 0)
            {
                System.Console.WriteLine("AAAAAAAAAA");

            }
            return View(persons);
        }
        public ActionResult Person(int? id)
        {
            if (id != null)
            {
                Person person = db.Person.Where(p => id == p.ID).FirstOrDefault();
                if (person != null)
                {
                    return View(person);
                }
                else
                {
                    return View("Error");
                }
            }
            else
            {
                return View("Error");
            }
        }
    }
}