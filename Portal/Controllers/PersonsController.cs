﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Models;

namespace Portal.Controllers
{
    public class PersonsController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        Person person1 = new Person
        {
            Second_Name = "Ivanov",
            First_Name = "Ivan"

        };
        Person person2 = new Person
        {
            Second_Name  = "Petrov",
            First_Name = "Petr"

        };
        Person person3 = new Person
        {
            Second_Name = "Lenova",
            First_Name = "Lena"

        };


        // GET: Articles
        public ActionResult Index()
        {
            //db.Person.Add(person1);
            //db.Person.Add(person2);
            //db.Person.Add(person3);
            //db.SaveChanges();
            Person[] persons = db.Person.ToArray();
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
                    return HttpNotFound();
                }
            }
            else
            {
                return HttpNotFound();
            }
        }
    }
}