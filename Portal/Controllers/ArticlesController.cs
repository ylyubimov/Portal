﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Portal.Controllers
{
    [RoutePrefix("articles")]
    public class ArticlesController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Articles

        [Authorize]
        [Route("{id:int}")]
        public ActionResult Index(int id)
        {
            Article article = db.Article.Where(p => id == p.ID).FirstOrDefault();
            if (article == null)
            {
                return HttpNotFound();
            }
             
            return View(article);
        }


        [Authorize]
        [HttpGet]
        [Route("{id:int}/edit")]
        public ActionResult Edit(int id)
        {
            Article article = db.Article.Where(p => id == p.ID).FirstOrDefault();
            if(article.Author.UserName != User.Identity.Name)
                return View("You are not the author of this article");
            if (article != null)
            {
                var Blogs = db.Blog.OrderBy(r => r.Name).ToList().Select(rr =>
                    new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name, Selected = article.Blogs.Contains(rr) }).ToList();
                ViewBag.Blogs = Blogs;
                return View(article);
            }
            else
            {
                return View("Error");
            }
        }

        [Authorize]
        [HttpPost]
        [Route("{id:int}/edit")]
        public ActionResult Edit(int id, string[] Blogs,Article articleEdit)
        {
            if (ModelState.IsValid)
                return View("Error");
            Article article = db.Article.Where(p => id == p.ID).FirstOrDefault();
            if (article != null)
            {
                if(article.Author.UserName == User.Identity.Name)
                {
                    article.Text = articleEdit.Text;
                    article.Name = articleEdit.Name;
                    //article.Blogs = db.Blog.Where(p => Blogs.Contains(p.Name) ).ToArray();
                    db.Entry(article).State = EntityState.Modified;

                    db.SaveChanges();
                    return RedirectToAction( "Index", "articles", article.ID);
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

        [Authorize]
        [HttpPost]
        public ActionResult AddComment(string comment, int id)
        {
            Article article = db.Article.Where(p => id == p.ID).FirstOrDefault();

            Person authorComment = db.Person.Where(p => User.Identity.Name == p.UserName).FirstOrDefault();
            //TODO: delete this line

            Comment c = new Comment();
            
            c.Text = comment;
            c.Article = article;
            c.Author = authorComment;
            c.Create_Time = DateTime.Now;

            article.Comments.Add(c);

            db.SaveChanges();                        
            return RedirectToAction("index", id);
        }
    }
}