﻿using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    [RoutePrefix("course")]
    public class CoursesController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        
        // GET: /Courses/
        [HttpGet]
        [Route("")]
        public ActionResult Index(int? grade, string basePart)
        {
            ViewBag.Title = "Courses";
            ViewBag.Grade = grade;
            ViewBag.BasePart = basePart;
            Course[] courses = new Course[] { };
            if (grade != null && basePart != null)
            {
                courses = db.Course.Where(p => p.BasePart == basePart && p.Grade == grade).ToArray();
            } else
            {
                if (grade != null)
                {
                    courses = db.Course.Where(p => p.Grade == grade).ToArray();
                }
                else
                {
                    if (basePart != null)
                    {
                        courses = db.Course.Where(p => p.BasePart == basePart).ToArray();
                    } else
                    {
                        courses = db.Course.ToArray();
                    }
                }
            }
            
            return View(courses);
        }

        [Route("{id:int}")]
        public ActionResult Course(int id)
        {
            Course course = db.Course.Include("Lessons").Where(p => id == p.ID).FirstOrDefault();
            if (course == null)
            {
                return HttpNotFound();
            }
            ViewBag.MailToAll = "";
            foreach(Person student in course.Students.ToArray()) {
                ViewBag.MailToAll += "<" + student.Email + ">,";
            }
            return View(course);
        }
        
        [HttpGet]
        [Route("{id:int}/edit")]
        [Authorize(Roles = "editor, admin")]
        public ActionResult Edit(int id)
        {
            var course = db.Course.Where(p => id == p.ID).FirstOrDefault();
            if (course == null)
            {
                return HttpNotFound();
            };
            return View(course);
        }
        
        [HttpPost]
        [Route("{id:int}/edit")]
        [Authorize(Roles = "editor, admin")]
        public ActionResult Edit(Course course)
        {
            return View();
        }

        [HttpPost]
        [Route("")]
        public ActionResult Index(string SearchFor)
        {
            ViewBag.Title = "Courses";
            ViewBag.SearchValue = SearchFor;
            var CourseList = db.Course.Where(x => x.Name.ToUpper().IndexOf(SearchFor.ToUpper()) >= 0).Take(50).ToArray();
            return View(CourseList);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddLesson(string Name, string Description, string Links, int id)
        {
            if (Name == null || Name == "")
            {
                Name = "Урок";
            }
            Course course = db.Course.Where(p => id == p.ID).FirstOrDefault();
            Person author = db.Users.Where(p => User.Identity.Name == p.UserName).FirstOrDefault();
            if (!course.Teachers.Contains(author))
            {
                return View("Error", "У вас нет прав на редактирование этих материалов");
            }
            var lesson = new Lesson() { Name = Name, Description = Description, Links = Links };
            course.Lessons.Add(lesson);
            
            db.SaveChanges();
            return RedirectToAction("Course", id);
        }


        [HttpGet]
        [Route("Create")]
        [Authorize(Roles = "editor, admin")]
        public ActionResult Create()
        {
            return View(new Course() { Name = "Name" });
        }

        [HttpPost]
        [Route("Create")]
        [Authorize(Roles = "editor, admin")]
        public ActionResult Create(Course newCourse)
        {
            //var course = db.Course.Add(newCourse);
            //course.Teachers.Add(db.Person.Where.Person)
            //db.SaveChanges();
            //if (course == null)
               // return View("Error");
            return RedirectToAction("Index", "Courses");
        }


        [HttpGet]
        [Route("{id:int}/EditLesson")]
        [Authorize(Roles = "editor, admin")]
        public ActionResult EditLesson(int id, int courseId)
        {
            Lesson lesson = db.Lesson.Where(l => l.ID == id).First();
            if (lesson == null)
            {
                return HttpNotFound();
            }

            Course course = db.Course.Where(c => c.ID == courseId).First();
            if (course.Teachers.Where(t => User.Identity.Name == t.UserName).FirstOrDefault() == null && !User.IsInRole("admin"))
            {
                return View("Error", "У вас нет прав на редактирование этих материалов");
            }
            ViewBag.CourseId = courseId;
            return View(lesson);
        }

        [HttpPost]
        [Route("{id:int}/EditLesson")]
        [Authorize(Roles = "editor, admin")]
        public ActionResult EditLesson(int courseId, int id, Lesson editedLesson)
        {
            Lesson lesson = db.Lesson.Where(l => l.ID == id).First();
            if (lesson == null)
            {
                return HttpNotFound();
            }

            Course course = db.Course.Where(c => c.ID == courseId).First();
            if (course.Teachers.Where(t => User.Identity.Name == t.UserName).FirstOrDefault() == null && !User.IsInRole("admin"))
            {
                return View("Error", "У вас нет прав на редактирование этих материалов");
            }

            lesson.Name = editedLesson.Name;
            lesson.Description = editedLesson.Description;
            lesson.Links = editedLesson.Links;
            
            if (!ModelState.IsValid)
            {
                ViewBag.CourseId = courseId;
                return View(lesson);
            }

            db.SaveChanges();

            return RedirectToAction("course", new { id = courseId });
        }

        [HttpGet]
        [Route("{lessonId:int}/RemoveLesson")]
        [Authorize(Roles = "editor, admin")]
        public ActionResult RemoveLesson(int courseId, int lessonId)
        {
            Lesson lesson = db.Lesson.Where(l => l.ID == lessonId).First();
            if (lesson == null)
            {
                return HttpNotFound();
            }

            Course course = db.Course.Where(c => c.ID == courseId).First();

            if (course.Teachers.Where(t => User.Identity.Name == t.UserName).FirstOrDefault() == null && !User.IsInRole("admin"))
            {
                return View("Error", "У вас нет прав на редактирование этих материалов");
            }

            course.Lessons.Remove(lesson);
            db.Lesson.Remove(lesson);

            db.SaveChanges();

            return RedirectToAction("course", new { id = courseId });
        }
    }
}