using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System.Net.Http;
using System.Net;

namespace Portal.Controllers
{
    [RoutePrefix("programs")]
    public class ProgramsController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Articles
        
        [Route("{id:int}")]
        public ActionResult Program(int id)
        {
            Program program = db.Program.Where(p => id == p.ID).FirstOrDefault();
            if (program == null)
            {
                return HttpNotFound();
            }

            return View(program);
        }

        public ActionResult Index()
        {
            var program = db.Program.ToArray();
            if (program == null)
            {
                return HttpNotFound();
            }

            return View(program);
        }

        [Authorize]
        [HttpGet]
        [Route("{id:int}/edit")]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id)
        {
            Program program = db.Program.Where(p => id == p.ID).FirstOrDefault();
            if (!User.IsInRole("admin"))
                return HttpNotFound();
            if (program != null)
            {
                var Courses = db.Course.OrderBy(r => r.Name).ToList().Select(rr =>
                    new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name, Selected = program.Courses.Contains(rr) }).ToList();
                ViewBag.Courses = Courses;
                return View(program);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [Authorize]
        [HttpPost]
        [Route("{id:int}/edit")]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id, string[] Courses, Program programEdit)
        {

            Program program = db.Program.Where(p => id == p.ID).FirstOrDefault();
            if (program != null)
            {
                if (User.IsInRole("admin"))
                {
                    program.Grade = programEdit.Grade;
                    program.Name = programEdit.Name;
                    program.Courses.Clear();
                    program.Courses = db.Course.Where(p => Courses.Contains(p.Name)).ToList();
                    db.SaveChanges();
                    return RedirectToAction("Index", "programs", program.ID);
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

        [Authorize]
        [HttpGet]
        [Route("Create")]
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            if (!User.IsInRole("admin"))
                return View("Error");
            var Courses = db.Course.OrderBy(r => r.Name).ToList().Select(rr =>
                      new SelectListItem { Value = rr.Name, Text = rr.Name, Selected = false }).ToList();
            ViewBag.Courses = Courses;
            return View(new Program() { Name = "Name" });
        }

        [Authorize]
        [HttpPost]
        [Route("Create")]
        [Authorize(Roles = "admin")]
        public ActionResult Create(string[] Courses, Program programEdit)
        {
            programEdit.Courses = db.Course.Where(p => Courses.Contains(p.Name)).ToArray();
            if (!User.IsInRole("admin"))
                return View("Error");
            var program = db.Program.Add(programEdit);
            db.SaveChanges();
            if (program == null)
                return View("Error");
            return RedirectToAction("Index", "programs", program.ID);
        }
    }
}