using Portal.Models;
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
                return View("Error");
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
            Course course = db.Course.Where(p => id == p.ID).FirstOrDefault();
            Person author = db.Users.Where(p => User.Identity.Name == p.UserName).FirstOrDefault();
            if(!course.Teachers.Contains(author))
                return View("Error");
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
            CourseCreateEdit c = new Models.CourseCreateEdit();
            c.Name = "Name";
            List<Tuple<Person, bool>> students = new List<Tuple<Person, bool>>();
            List<Tuple<Person, bool>> teachers = new List<Tuple<Person, bool>>();
            foreach (Person p in db.Users)
            {
                if (p.Person_Type == "Teacher")
                {
                    teachers.Add(new Tuple<Person, bool>(p, false));
                }
                else if (p.Person_Type == "Student")
                {
                    students.Add(new Tuple<Person, bool>(p, false));
                }
            }
            c.Teachers = teachers;
            c.Students = students;
            return View(c);
        }

        [HttpPost]
        [Route("Create")]
        [Authorize(Roles = "editor, admin")]
        public ActionResult Create(CourseCreateEdit newCourse)
        {
            List<Person> students = new List<Person>() ;
            List<Person> teachers = new List<Person>() ;

            foreach (Tuple<Person, bool> t in newCourse.Students) {
                if (t.Item2)
                {
                    students.Add(db.Users.Where(p=> p.Id == t.Item1.Id).FirstOrDefault());
                }
            }
            foreach (Tuple<Person, bool> t in newCourse.Teachers)
            {
                if (t.Item2)
                {
                    students.Add(db.Users.Where(p => p.Id == t.Item1.Id).FirstOrDefault());
                }
            }

            var course = new Course { Name = newCourse.Name, Description = newCourse.Description,  Date_and_Time = newCourse.Date_and_Time, Place = newCourse.Place, Number_of_Classes = newCourse.Number_of_Classes, Number_of_Hours = newCourse.Number_of_Hours, Report_Type = newCourse.Report_Type, Report_Date = newCourse.Report_Date, Students = students, Teachers = teachers }  ;

            db.Course.Add(course);
            db.SaveChanges();
            return RedirectToAction("Index", "Courses");
        }
    }
}