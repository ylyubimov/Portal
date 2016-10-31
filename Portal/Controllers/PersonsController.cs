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
    [RoutePrefix( "persons" )]
    public class PersonsController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        private Person[] getTeachers( Person[] persons )
        {
            return persons.Where( p => p.Person_Type == "Teacher" ).ToArray();
        }

        private Person[] getStudents( Person[] persons )
        {
            return persons.Where( p => p.Person_Type == "Student" ).ToArray();
        }

        // GET: Articles
        [HttpGet]
        [Route( "" )]
        public ActionResult Index( string view, string type )
        {
            var list = db.Roles.OrderBy( r => r.Name ).ToList().Select( rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name } ).ToList();
            ViewBag.Roles = list;
            ViewBag.Title = "People";
            ViewBag.ExtendType = "person";
            Person[] AllPersons = db.Users.ToArray();
            Person[][] persons = new Person[][] { getTeachers( AllPersons ), getStudents( AllPersons ) };
            ViewBag.Type = type;

            if( view != null && view == "grid" ) {
                ViewBag.View = "grid";
            }
            return View( persons );
        }

        [Route( "{id}" )]
        public ActionResult Person( string id )
        {
            if( id != "" ) {
                Person person = db.Users.Where( p => id == p.Id ).FirstOrDefault();
                if( person != null ) {
                    return View( person );
                } else {
                    return HttpNotFound();
                }
            } else {
                return HttpNotFound();
            }
        }

        [HttpPost]
        [Route( "" )]
        public ActionResult Index( string SearchFor )
        {
            if( SearchFor != "" ) {
                ViewBag.Title = "People";
                ViewBag.SearchValue = SearchFor;
                var PersonList = db.Users.Where( x => ( x.First_Name + " " + x.Second_Name ).ToUpper().IndexOf( SearchFor.ToUpper() ) >= 0 ||
                                                        ( x.First_Name + " " + x.Middle_Name + " " + x.Second_Name ).ToUpper().IndexOf( SearchFor.ToUpper() ) >= 0
                                                       ).Take( 50 ).ToArray();
                Person[][] persons = new Person[][] { getTeachers( PersonList ), getStudents( PersonList ) };
                return View( persons );
            } else {
                var list = db.Roles.OrderBy( r => r.Name ).ToList().Select( rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name } ).ToList();
                ViewBag.Roles = list;
                ViewBag.Title = "People";
                ViewBag.ExtendType = "person";
                Person[] AllPersons = db.Users.ToArray();
                Person[][] persons = new Person[][] { getTeachers( AllPersons ), getStudents( AllPersons ) };
                return View( persons );
            }
        }

        [HttpGet]
        [Route( "{id}/edit" )]
        [Authorize]
        public ActionResult Edit( string id )
        {
            Person person = db.Users.Where( x => x.Id == id ).First();
            if( person == null || !User.IsInRole( "admin" ) && User.Identity.Name != person.UserName ) {
                return HttpNotFound();
            }
            if( person.Picture == null || person.Picture.Name == "DefaultPicture" ) {
                person.Picture = new Picture {
                    URL = null
                };
            }
            return View( person );
        }

        [HttpPost]
        [Route( "{id}/edit" )]
        [Authorize]
        public ActionResult Edit( string id, Person editedPerson )
        {
            Person person = db.Users.Where( p => p.Id == id ).FirstOrDefault();
            if( person != null ) {
                if( person.UserName == User.Identity.Name || User.IsInRole( "admin" ) ) {
                    person.First_Name = editedPerson.First_Name;
                    person.Middle_Name = editedPerson.Middle_Name;
                    person.Second_Name = editedPerson.Second_Name;
                    person.PhoneNumber = editedPerson.PhoneNumber;
                    person.Email = editedPerson.Email;
                    if( editedPerson.Picture.URL != null ) {
                        person.Picture = new Picture {
                            URL = editedPerson.Picture.URL,
                            Name = person.UserName + "picture"
                        };
                    } else {
                        person.Picture = db.Picture.Where( p => p.Name == "DefaultPicture" ).First();
                    }
                }
            }
            db.SaveChanges();
            return RedirectToAction( "person", new { @id = id } );
        }

        [HttpPost]
        [Route( "upload" )]
        public JsonResult Upload()
        {
            string numberImg = Request.Form[0];
            string id = Request.Form[1];
            string imgPath = null;
            foreach( string file in Request.Files ) {
                var upload = Request.Files[file];
                if( upload != null ) {
                    imgPath = System.IO.Path.GetFileName( upload.FileName );
                    upload.SaveAs( Server.MapPath( "~/personsImages/" + imgPath ) );
                    Person person = db.Users.Where( p => p.Id == id ).FirstOrDefault();
                    person.Picture = new Picture {
                        URL = "/personsImages/" + imgPath,
                        Name = person.UserName + "picture"
                    };
                }
            }
            db.SaveChanges();
            string[] imageData = { imgPath, numberImg };
            return Json( imageData );
        }
    }
}