using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    [RoutePrefix( "courseInstances" )]
    public class CourseInstancesController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();


        // GET: CourseInstance
        public ActionResult Index()
        {
            return View();
        }

        [Route( "{id:int}" )]
        public ActionResult CourseInstance( int id )
        {
            CourseInstance courseInstance = db.CourseInstance.Where( p => id == p.ID ).FirstOrDefault();
            if( courseInstance == null ) {
                return HttpNotFound();
            }
            return View( courseInstance );
        }

        [HttpGet]
        [Route( "{id:int}/Edit" )]
        [Authorize( Roles = "editor, admin" )]
        public ActionResult Edit( int courseId, int id )
        {
            ViewBag.CourseID = courseId;
            CourseInstance courseInstance = db.CourseInstance.Where( ci => ci.ID == id ).First();
            if( courseInstance == null ) {
                return HttpNotFound();
            }

            Course course = db.Course.Where( cs => cs.ID == courseId ).First();
            if( course.Teachers.Where( t => User.Identity.Name == t.UserName ).FirstOrDefault() == null && !User.IsInRole( "admin" ) ) {
                ViewBag.Message = "У вас нет прав на редактирование этих материалов";
                return View( "Error" );
            }

            CourseInstanceCreateEdit c = new Models.CourseInstanceCreateEdit();
            c.ID = courseInstance.ID;
            c.Year = courseInstance.Year;
            c.BaseCourse = courseInstance.BaseCourse;
            c.AdditionalDescription = courseInstance.AdditionalDescription;
            c.Place = courseInstance.Place;
            c.Report_Date = courseInstance.Report_Date;
            List<Person> students = new List<Person>();
            foreach( Person p in db.Users ) {
                if( p.Person_Type == "Student" ) {
                    students.Add( p );
                }
            }
            c.Chosen_Students = new bool[students.Count];
            c.Students = students.ToArray();
            for( int i = 0; i < students.Count; i++ ) {
                if( courseInstance.Students != null ) {
                    if( courseInstance.Students.Where( t => t.Id == c.Students[i].Id ).FirstOrDefault() != null ) {
                        c.Chosen_Students[i] = true;
                    } else {
                        c.Chosen_Students[i] = false;
                    }
                } else {
                    c.Chosen_Students[i] = false;
                }
            }
            return View( c );
        }

        [HttpPost]
        [Route( "{id:int}/Edit" )]
        [Authorize( Roles = "editor, admin" )]
        public ActionResult Edit( int courseId, int id, CourseInstanceCreateEdit editedCourseInstance )
        {
            if( editedCourseInstance.Year < 0 || editedCourseInstance.BaseCourse == null ) {
                return View( editedCourseInstance );
            }

            List<Person> studentsList = new List<Person>();

            for( int i = 0; i < editedCourseInstance.Chosen_Students.Count(); i++ ) {
                if( editedCourseInstance.Chosen_Students[i] ) {
                    string studentId = editedCourseInstance.Students[i].Id;
                    Person student = db.Users.Where( p => p.Id == studentId ).FirstOrDefault();
                    studentsList.Add( student );
                }
            }
            var courseInstance = db.CourseInstance.Where( c => c.ID == editedCourseInstance.ID ).First();
            if( courseInstance == null ) {
                return HttpNotFound();
            }
            courseInstance.Year = editedCourseInstance.Year;
            courseInstance.AdditionalDescription = editedCourseInstance.AdditionalDescription;
            courseInstance.BaseCourse = editedCourseInstance.BaseCourse;
            courseInstance.Place = editedCourseInstance.Place;
            courseInstance.Report_Date = editedCourseInstance.Report_Date;
            courseInstance.Students.Clear();
            courseInstance.Students = studentsList;
            db.SaveChanges();
            return RedirectToAction( "CourseInstance", new { id = id } );
        }

        [HttpGet]
        [Route( "{id:int}/Remove" )]
        [Authorize( Roles = "editor, admin" )]
        public ActionResult Remove( int courseId, int id )
        {
            CourseInstance courseInstance = db.CourseInstance.Where( c => c.ID == id ).First();
            if( courseInstance == null ) {
                return HttpNotFound();
            }

            Course course = db.Course.Where( c => c.ID == courseId ).First();

            if( course.Teachers.Where( t => User.Identity.Name == t.UserName ).FirstOrDefault() == null && !User.IsInRole( "admin" ) ) {
                ViewBag.Message = "У вас нет прав на редактирование этих материалов";
                return View( "Error" );
            }

            db.CourseInstance.Remove( courseInstance );

            db.SaveChanges();

            return RedirectToAction( "course", new { id = courseId } );
        }

        [HttpGet]
        [Route( "{courseId:int}/Create" )]
        [Authorize( Roles = "editor, admin" )]
        public ActionResult Create( int courseId )
        {
            ViewBag.CourseID = courseId;
            CourseInstanceCreateEdit c = new Models.CourseInstanceCreateEdit();
            List<Person> studentsList = new List<Person>();
            foreach( Person p in db.Users ) {
                if( p.Person_Type == "Student" ) {
                    studentsList.Add( p );
                }
            }
            c.Chosen_Students = new bool[studentsList.Count];
            for( int i = 0; i < studentsList.Count; i++ ) {
                c.Chosen_Students[i] = false;
            }
            c.Students = studentsList.ToArray();
            return View( c );
        }

        [HttpPost]
        [Route( "{courseId:int}/Create" )]
        [Route( "Create" )]
        [Authorize( Roles = "editor, admin" )]
        public ActionResult Create( int courseId, CourseInstanceCreateEdit newCourseInstance )
        {
            newCourseInstance.BaseCourse = db.Course.Where( c => c.ID == courseId ).FirstOrDefault();
            if( newCourseInstance.Year < 0 || newCourseInstance.BaseCourse == null ) {
                return View( newCourseInstance );
            }
            List<Person> studentsList = new List<Person>();
            for( int i = 0; i < newCourseInstance.Chosen_Students.Count(); i++ ) {
                if( newCourseInstance.Chosen_Students[i] ) {
                    string id = newCourseInstance.Students[i].Id;
                    Person student = db.Users.Where( p => p.Id == id ).FirstOrDefault();
                    studentsList.Add( student );
                }
            }

            var courseInstance = new CourseInstance {
                Year = newCourseInstance.Year,
                AdditionalDescription = newCourseInstance.AdditionalDescription,
                BaseCourse = newCourseInstance.BaseCourse,
                Place = newCourseInstance.Place,
                Report_Date = newCourseInstance.Report_Date,
                Students = studentsList,
            };

            db.CourseInstance.Add( courseInstance );
            db.SaveChanges();
            return RedirectToAction( courseId.ToString(), "course" );
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddLesson( string Name, string Description, string Links, int id )
        {
            if( Name == null || Name == "" ) {
                Name = "Урок";
            }
            CourseInstance courseInstance = db.CourseInstance.Where( p => id == p.ID ).FirstOrDefault();
            Person author = db.Users.Where( p => User.Identity.Name == p.UserName ).FirstOrDefault();
            if( !courseInstance.BaseCourse.Teachers.Contains( author ) ) {
                ViewBag.Message = "У вас нет прав на редактирование этих материалов";
                return View( "Error" );
            }
            var lesson = new Lesson() { Name = Name, Description = Description, Links = Links };
            courseInstance.Lessons.Add( lesson );

            db.SaveChanges();
            return RedirectToAction( "CourseInstance", id );
        }

        [HttpGet]
        [Route( "{id:int}/EditLesson" )]
        [Authorize( Roles = "editor, admin" )]
        public ActionResult EditLesson( int id, int courseInstanceId )
        {
            Lesson lesson = db.Lesson.Where( l => l.ID == id ).First();
            if( lesson == null ) {
                return HttpNotFound();
            }

            CourseInstance courseInstance = db.CourseInstance.Where( c => c.ID == courseInstanceId ).First();
            if( courseInstance.BaseCourse.Teachers.Where( t => User.Identity.Name == t.UserName ).FirstOrDefault() == null && !User.IsInRole( "admin" ) ) {
                ViewBag.Message = "У вас нет прав на редактирование этих материалов";
                return View( "Error" );
            }
            ViewBag.CourseInstanceId = courseInstanceId;
            return View( lesson );
        }

        [HttpPost]
        [Route( "{id:int}/EditLesson" )]
        [Authorize( Roles = "editor, admin" )]
        public ActionResult EditLesson( int courseInstanceId, int id, Lesson editedLesson )
        {
            Lesson lesson = db.Lesson.Where( l => l.ID == id ).First();
            if( lesson == null ) {
                return HttpNotFound();
            }

            CourseInstance courseInstance = db.CourseInstance.Where( c => c.ID == courseInstanceId ).First();
            if( courseInstance.BaseCourse.Teachers.Where( t => User.Identity.Name == t.UserName ).FirstOrDefault() == null && !User.IsInRole( "admin" ) ) {
                ViewBag.Message = "У вас нет прав на редактирование этих материалов";
                return View( "Error" );
            }

            lesson.Name = editedLesson.Name;
            lesson.Description = editedLesson.Description;
            lesson.Links = editedLesson.Links;

            if( !ModelState.IsValid ) {
                ViewBag.CourseId = courseInstanceId;
                return View( lesson );
            }

            db.SaveChanges();

            return RedirectToAction( "CourseInstance", new { id = courseInstanceId } );
        }

        [HttpGet]
        [Route( "{lessonId:int}/RemoveLesson" )]
        [Authorize( Roles = "editor, admin" )]
        public ActionResult RemoveLesson( int courseInstanceId, int lessonId )
        {
            Lesson lesson = db.Lesson.Where( l => l.ID == lessonId ).First();
            if( lesson == null ) {
                return HttpNotFound();
            }

            CourseInstance courseInstance = db.CourseInstance.Where( c => c.ID == courseInstanceId ).First();

            if( courseInstance.BaseCourse.Teachers.Where( t => User.Identity.Name == t.UserName ).FirstOrDefault() == null && !User.IsInRole( "admin" ) ) {
                ViewBag.Message = "У вас нет прав на редактирование этих материалов";
                return View( "Error" );
            }

            db.Lesson.Remove( lesson );

            db.SaveChanges();

            return RedirectToAction( "CourseInstance", new { id = courseInstanceId } );
        }

    }
}