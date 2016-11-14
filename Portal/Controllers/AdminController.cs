using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net.Mail;
using System.Collections.Specialized;
using System.Configuration;

namespace Portal.Controllers
{
    public class MyViewModel
    {
        public Course[] courses { get; set; }
        public HashSet<int> checkedIds { get; set; }
        public string personId { get; set; }
    }

    [RoutePrefix( "admin" )]
    public class AdminController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Articles

        [Authorize( Roles = "admin" )]
        [Route( "" )]
        [HttpGet]
        public ActionResult AdminTable()
        {
            Person[] allPersons = db.Users.Where( p => true == p.Exists ).ToArray();
            Array.Sort( allPersons, new Comparison<Person>( ( x, y ) => String.Compare( x.Second_Name, y.Second_Name ) ) );
            ViewBag.Title = "Люди";
            return View( allPersons );
        }

        [Authorize( Roles = "admin" )]
        [HttpPost]
        public ActionResult Save( string id, string[] checkedCourses )
        {
            Person person = db.Users.Where( p => id == p.Id ).FirstOrDefault();
            person.Subscribed_Courses.Clear();
            if( checkedCourses != null ) {
                foreach( string name in checkedCourses ) {
                    person.Subscribed_Courses.Add( db.CourseInstance.Where( c => name == c.BaseCourse.Name ).FirstOrDefault() );
                }
            }
            db.SaveChanges();
            return RedirectToAction( "AdminTable" );
        }

        [Authorize( Roles = "admin" )]
        [Route( "{id}" )]
        [HttpGet]
        public ActionResult Index( string id )
        {
            Course[] allCourses = db.Course.ToArray();
            Person person = db.Users.Where( p => id == p.Id ).FirstOrDefault();
            CourseInstance[] studentCourses = person.Subscribed_Courses.ToArray();
            HashSet<int> idCoursesSet = new HashSet<int>();
            if( studentCourses != null ) {
                foreach( CourseInstance c in studentCourses ) {
                    idCoursesSet.Add( c.ID );
                }
            }
            var viewModel = new MyViewModel {
                courses = allCourses,
                checkedIds = idCoursesSet,
                personId = id
            };

            return View( viewModel );
        }


        [Authorize( Roles = "admin" )]
        [HttpPost]
        public ActionResult Delete( string id )
        {
            Person person = db.Users.Where( p => id == p.Id ).FirstOrDefault();
            var userManager = new ApplicationUserManager( new UserStore<Person>( db ) );
            Person.DeleteUser( db, id );
            db.SaveChanges();
            return RedirectToAction( "AdminTable" );
        }

        protected void SendEmailToVerificatedUser(Person user)
        {
            try
            {
                NameValueCollection mailingSection = (NameValueCollection)ConfigurationManager.GetSection("adminMailingSettings");

                string senderEmail = mailingSection["FromEmailAddress"].ToString(); // address that makes all the mailing
                string senderPasswd = mailingSection["FromEmailPassword"].ToString();
                string senderDisplayName = mailingSection["FromEmailDisplayName"].ToString();

                MailMessage mail = new MailMessage();
                mail.To.Add(user.Email);
                mail.From = new MailAddress(senderEmail, senderDisplayName, System.Text.Encoding.UTF8);
                mail.Subject = "[ABBYY Portal] You were approved as a teacher";
                mail.SubjectEncoding = System.Text.Encoding.UTF8;

                string bodyTemplate = "Hello, {0}!\r\n\r\n" +
                "You were just approved as a teacher and now you are able to start working on courses.\r\n\r\n" +
                "Yours, ABBYY Portal Team.";
                string fullName = String.Format("{0} {1} {2}", user.First_Name, user.Middle_Name, user.Second_Name);

                mail.Body = String.Format(bodyTemplate, fullName);
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = false;
                mail.Priority = MailPriority.High;
                SmtpClient client = new SmtpClient();
                client.Credentials = new System.Net.NetworkCredential(senderEmail, senderPasswd);
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;

                client.Send(mail);
            }
            catch (Exception ex)
            {
                Exception ex2 = ex;
                List<string> errorMessages = new List<string>();
                while (ex2 != null)
                {
                    errorMessages.Add(ex2.ToString());
                    ex2 = ex2.InnerException;
                }
                IdentityResult result = new IdentityResult(errorMessages);
                // should log it somewhere
            }
        }

        [Authorize( Roles = "admin" )]
        [HttpPost]
        public ActionResult ChangeType( string id )
        {
            Person person = db.Users.Where( p => id == p.Id ).FirstOrDefault();

            if( person.Person_Type == "Teacher" ) {
                person.Person_Type = "Admin";
            } else if( person.Person_Type == "Admin" ) {
                person.Person_Type = "Student";
            } else {
                if (person.Is_Waiting_Approval)
                {
                    person.Is_Waiting_Approval = false;
                    person.Person_Type = "Teacher";
                    db.SaveChanges();
                    SendEmailToVerificatedUser(person);
                    return RedirectToAction("AdminTable");
                }
                person.Person_Type = "Teacher";
            }
            db.SaveChanges();
            return RedirectToAction( "AdminTable" );
        }
        [Authorize( Roles = "admin" )]
        [HttpPost]
        public ActionResult ChangeRole( string id )
        {
            Person p = db.Users.Where( pp => id == pp.Id ).FirstOrDefault();

            var userManager = new ApplicationUserManager( new UserStore<Person>( db ) );

            if( userManager.IsInRole( p.Id, "editor" ) ) {
                userManager.RemoveFromRole( p.Id, "editor" );
                userManager.AddToRole( p.Id, "admin" );
            } else if( userManager.IsInRole( p.Id, "admin" ) ) {
                userManager.RemoveFromRole( p.Id, "admin" );
                userManager.AddToRole( p.Id, "user" );
            } else {
                userManager.RemoveFromRole( p.Id, "user" );
                userManager.AddToRole( p.Id, "editor" );
            }

            db.SaveChanges();

            return RedirectToAction( "AdminTable" );
        }
    }
}