using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    [RoutePrefix( "course" )]
    public class CoursesController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Courses/
        [HttpGet]
        [Route( "" )]
        public ActionResult Index( int? grade, string basePart )
        {
            ViewBag.Title = "Courses";
            ViewBag.Grade = grade;
            ViewBag.BasePart = basePart;
            Course[] courses = new Course[] { };
            if( grade != null && basePart != null ) {
                var prog = db.Program.Where( p => p.Name == grade.ToString() + " курс" ).FirstOrDefault();
                if( prog != null )
                    courses = prog.Courses.Where( p => p.BasePart == basePart || p.BasePart == null ).ToArray();
                else
                    courses = db.Course.ToArray();
            } else {
                if( grade != null ) {
                    var prog = db.Program.Where( p => p.Name == grade.ToString() + " курс" ).FirstOrDefault();
                    prog.Courses.ToArray();
                    if( prog != null )
                        courses = prog.Courses.ToArray();
                    else
                        courses = db.Course.ToArray();
                } else {
                    if( basePart != null ) {
                        courses = db.Course.Where( p => p.BasePart == basePart || p.BasePart == null ).ToArray();
                    } else {
                        courses = db.Course.ToArray();
                    }
                }
            }

            return View( courses );
        }

        [Route( "{id:int}" )]
        public ActionResult Course( int id )
        {
            Course course = db.Course.Include( "CourseInstances" ).Where( p => id == p.ID ).FirstOrDefault();
            if( course == null ) {
                return HttpNotFound();
            }
            ViewBag.MailToAll = "";
            return View( course );
        }


        [HttpPost]
        [Route( "" )]
        public ActionResult Index( string SearchFor )
        {
            ViewBag.Title = "Courses";
            ViewBag.SearchValue = SearchFor;
            var CourseList = db.Course.Where( x => x.Name.ToUpper().IndexOf( SearchFor.ToUpper() ) >= 0 ).Take( 50 ).ToArray();
            return View( CourseList );
        }

        [HttpGet]
        [Route( "Create" )]
        [Authorize( Roles = "editor, admin" )]
        public ActionResult Create()
        {
            CourseCreateEdit c = new Models.CourseCreateEdit();
            c.Name = "Name";
            List<Person> studentsList = new List<Person>();
            List<Person> teachersList = new List<Person>();
            List<Program> programsList = new List<Program>();
            List<Blog> blogsList = new List<Blog>();
            foreach( Person p in db.Users ) {
                if( p.Person_Type == "Teacher" ) {
                    teachersList.Add( p );
                } else if( p.Person_Type == "Student" ) {
                    studentsList.Add( p );
                }
            }
            foreach( Program p in db.Program ) {
                programsList.Add( p );
            }
            foreach( Blog b in db.Blog ) {
                blogsList.Add( b );
            }
            c.Chosen_Teachers = new bool[teachersList.Count];
            c.Chosen_Programs = new bool[programsList.Count];
            c.Chosen_Blogs = new bool[blogsList.Count];
            for( int i = 0; i < teachersList.Count; i++ ) {
                c.Chosen_Teachers[i] = false;
                if( User.IsInRole( "editor" ) || User.IsInRole( "admin" ) ) {
                    if( teachersList[i].UserName == User.Identity.Name ) {
                        c.Chosen_Teachers[i] = true;
                    }
                }
            }
            for( int i = 0; i < programsList.Count; i++ ) {
                c.Chosen_Programs[i] = false;
            }
            for( int i = 0; i < blogsList.Count; i++ ) {
                c.Chosen_Blogs[i] = false;
            }
            c.Blogs = blogsList.ToArray();
            c.Teachers = teachersList.ToArray();
            c.Programs = programsList.ToArray();
            return View( c );
        }

        [HttpPost]
        [Route( "Create" )]
        [Authorize( Roles = "editor, admin" )]
        public ActionResult Create( CourseCreateEdit newCourse )
        {

            if( newCourse.Name == null || newCourse.Number_of_Hours < 0 || newCourse.Number_of_Classes < 0 || newCourse.Description == null ) {
                return View( newCourse );
            }
            List<Person> studentsList = new List<Person>();
            List<Person> teachersList = new List<Person>();
            List<Program> programList = new List<Program>();
            List<Blog> blogsList = new List<Blog>();
            for( int i = 0; i < newCourse.Chosen_Teachers.Count(); i++ ) {
                if( newCourse.Chosen_Teachers[i] ) {
                    string id = newCourse.Teachers[i].Id;
                    Person teacher = db.Users.Where( p => p.Id == id ).FirstOrDefault();
                    teachersList.Add( teacher );
                }
            }
            for( int i = 0; i < newCourse.Chosen_Programs.Count(); i++ ) {
                if( newCourse.Chosen_Programs[i] ) {
                    int id = newCourse.Programs[i].ID;
                    Program program = db.Program.Where( p => p.ID == id ).FirstOrDefault();
                    programList.Add( program );
                }
            }

            for( int i = 0; i < newCourse.Chosen_Blogs.Count(); i++ ) {
                if( newCourse.Chosen_Blogs[i] ) {
                    int id = newCourse.Blogs[i].ID;
                    Blog blog = db.Blog.Where( p => p.ID == id ).FirstOrDefault();
                    blogsList.Add( blog );
                }
            }

            var course = new Course {
                Name = newCourse.Name,
                Description = newCourse.Description,
                BasePart = newCourse.Base_Part,
                Date_and_Time = newCourse.Date_and_Time,
                Number_of_Classes = newCourse.Number_of_Classes,
                Number_of_Hours = newCourse.Number_of_Hours,
                Report_Type = newCourse.Report_Type,
                Teachers = teachersList,
                Programs = programList,
                Blogs = blogsList
            };
            if( newCourse.Chosen_Programs.ToArray().Contains( true ) )
                course.Grade = newCourse.Chosen_Programs.ToList().IndexOf( true ) + 3;

            db.Course.Add( course );
            db.SaveChanges();
            return RedirectToAction( "Index", "Courses" );
        }

        [HttpGet]
        [Route( "{id:int}/Delete" )]
        [Authorize( Roles = "editor, admin" )]
        public ActionResult Delete( int id )
        {
            Course course = db.Course.Where( c => c.ID == id ).FirstOrDefault();
            if( course == null ) {
                return HttpNotFound();
            }
            course.Teachers.Clear();
            course.Programs.Clear();
            course.Blogs.Clear();
            db.Course.Remove( course );
            db.SaveChanges();
            return RedirectToAction( "Index", "Courses" );
        }

        [HttpGet]
        [Route( "{id:int}/edit" )]
        [Authorize( Roles = "editor, admin" )]
        public ActionResult Edit( int id )
        {

            Course course = db.Course.Where( crse => crse.ID == id ).FirstOrDefault();
            CourseCreateEdit c = new Models.CourseCreateEdit();
            c.ID = course.ID;
            c.Name = course.Name;
            c.Date_and_Time = course.Date_and_Time;
            c.Description = course.Description;
            c.Number_of_Classes = course.Number_of_Classes;
            c.Number_of_Hours = course.Number_of_Hours;
            c.Report_Type = course.Report_Type;
            c.Base_Part = course.BasePart;
            List<Person> students = new List<Person>();
            List<Person> teachers = new List<Person>();
            List<Program> programs = new List<Program>();
            List<Blog> blogs = new List<Blog>();
            foreach( Person p in db.Users ) {
                if( p.Person_Type == "Teacher" ) {
                    teachers.Add( p );
                } else if( p.Person_Type == "Student" ) {
                    students.Add( p );
                }
            }
            foreach( Blog b in db.Blog ) {
                blogs.Add( b );
            }
            foreach( Program p in db.Program ) {
                programs.Add( p );
            }
            c.Chosen_Teachers = new bool[teachers.Count];
            c.Chosen_Programs = new bool[programs.Count];
            c.Chosen_Blogs = new bool[blogs.Count];
            c.Teachers = teachers.ToArray();
            c.Programs = programs.ToArray();
            c.Blogs = blogs.ToArray();
            for( int i = 0; i < teachers.Count; i++ ) {
                if( course.Teachers != null ) {
                    if( course.Teachers.Where( t => t.Id == c.Teachers[i].Id ).FirstOrDefault() != null ) {
                        c.Chosen_Teachers[i] = true;
                    } else {
                        c.Chosen_Teachers[i] = false;
                    }
                } else {
                    c.Chosen_Teachers[i] = false;
                }
            }
            for( int i = 0; i < course.Programs.Count; i++ ) {
                if( course.Programs != null ) {
                    if( course.Programs.Where( t => t.ID == c.Programs[i].ID ).FirstOrDefault() != null ) {
                        c.Chosen_Programs[i] = true;
                    } else {
                        c.Chosen_Programs[i] = false;
                    }
                } else {
                    c.Chosen_Programs[i] = false;
                }
            }
            for( int i = 0; i < course.Blogs.Count; i++ ) {
                if( course.Blogs != null ) {
                    if( course.Blogs.Where( t => t.ID == c.Blogs[i].ID ).FirstOrDefault() != null ) {
                        c.Chosen_Blogs[i] = true;
                    } else {
                        c.Chosen_Blogs[i] = false;
                    }
                } else {
                    c.Chosen_Blogs[i] = false;
                }
            }
            return View( c );
        }

        [HttpPost]
        [Route( "{id:int}/edit" )]
        [Authorize( Roles = "editor, admin" )]
        public ActionResult Edit( CourseCreateEdit newCourse )
        {
            if( newCourse.Name == "" || newCourse.Number_of_Hours < 0 || newCourse.Number_of_Classes < 0 || newCourse.Description == null ) {
                return View( newCourse );
            }
            List<Person> studentsList = new List<Person>();
            List<Person> teachersList = new List<Person>();
            List<Program> programList = new List<Program>();
            List<Blog> blogsList = new List<Blog>();
            for( int i = 0; i < newCourse.Chosen_Teachers.Count(); i++ ) {
                if( newCourse.Chosen_Teachers[i] ) {
                    string id = newCourse.Teachers[i].Id;
                    Person teacher = db.Users.Where( p => p.Id == id ).FirstOrDefault();
                    teachersList.Add( teacher );
                }
            }
            for( int i = 0; i < newCourse.Chosen_Programs.Count(); i++ ) {
                if( newCourse.Chosen_Programs[i] ) {
                    int id = newCourse.Programs[i].ID;
                    Program program = db.Program.Where( p => p.ID == id ).FirstOrDefault();
                    programList.Add( program );
                }
            }
            for( int i = 0; i < newCourse.Chosen_Blogs.Count(); i++ ) {
                if( newCourse.Chosen_Blogs[i] ) {
                    int id = newCourse.Programs[i].ID;
                    Blog blog = db.Blog.Where( p => p.ID == id ).FirstOrDefault();
                    blogsList.Add( blog );
                }
            }

            var course = db.Course.Where( c => c.ID == newCourse.ID ).First();
            if( course == null ) {
                return HttpNotFound();
            }
            course.Name = newCourse.Name;
            course.Description = newCourse.Description;
            course.BasePart = newCourse.Base_Part;
            course.Date_and_Time = newCourse.Date_and_Time;
            course.Number_of_Classes = newCourse.Number_of_Classes;
            course.Number_of_Hours = newCourse.Number_of_Hours;
            course.Report_Type = newCourse.Report_Type;
            course.Teachers.Clear();
            course.Programs.Clear();
            course.Teachers = teachersList;
            course.Programs = programList;
            course.Blogs = blogsList;
            db.SaveChanges();
            return RedirectToAction( "Index", "Courses" );
        }
    }
}