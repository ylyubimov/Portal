﻿using System;
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
    [RoutePrefix( "articles" )]
    public class ArticlesController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Articles

        [Route( "{id:int}" )]
        public ActionResult Index( int id, int? position )
        {
            Article article = db.Article.Where( p => id == p.ID ).FirstOrDefault();
            if( article == null ) {
                return HttpNotFound();
            }

            return View( new ArticleWithPosition() { ArticleInfo = article, Position = position } );
        }


        [Authorize]
        [HttpGet]
        [Route( "{id:int}/edit" )]
        public ActionResult Edit( int id )
        {
            Article article = db.Article.Where( p => id == p.ID ).FirstOrDefault();
            if( article.Author.UserName != User.Identity.Name && !User.IsInRole( "admin" ) )
                return HttpNotFound();
            if( article != null ) {
                return View( article );
            } else {
                return HttpNotFound();
            }
        }

        [Authorize]
        [HttpPost]
        [Route( "{id:int}/edit" )]
        public ActionResult Edit( int id, Article articleEdit )
        {

            Article article = db.Article.Where( p => id == p.ID ).FirstOrDefault();
            if( article != null ) {
                if( article.Author.UserName == User.Identity.Name || User.IsInRole( "admin" ) ) {
                    article.Text = articleEdit.Text;
                    article.Name = articleEdit.Name;
                    //Какой-то неопознанный баг, пришлось костылить, простите меня(
                    ModelState["Author"].Errors.Clear();
                    if( !ModelState.IsValid ) {
                        return View( article );
                    }

                    db.SaveChanges();
                    return RedirectToAction( "Index", "articles", article.ID );
                } else {
                    return HttpNotFound();
                }
            } else {
                return HttpNotFound();
            }
        }


        [HttpGet]
        [Route( "{blogId:int}/Create" )]
        [Authorize( Roles = "editor, admin" )]
        public ActionResult Create( int blogId )
        {
            ViewBag.BlodID = blogId;
            var um = new UserManager<Person>( new UserStore<Person>( db ) );
            var author = um.FindByName( User.Identity.Name );
            if( author == null )
                return View( "Can't find your account in persons" );
            return View( new Article() { Name = "", Text = "", Author = author } );
        }

        [HttpPost]
        [Route( "Create" )]
        [Authorize( Roles = "editor, admin" )]
        [Route( "{blogId:int}/Create" )]
        public ActionResult Create( int blogId, Article articleEdit )
        {
            articleEdit.Blogs = db.Blog.Where( p => p.ID == blogId ).ToArray();
            var um = new UserManager<Person>( new UserStore<Person>( db ) );
            var author = um.FindByName( User.Identity.Name );
            if( author == null )
                return View( "Error", "Can't find your account in persons" );
            articleEdit.Author = author;
            articleEdit.Date_of_Creation = DateTime.Now;
            articleEdit.Likes_Count = 0;
            articleEdit.Dislikes_Count = 0;
            var article = db.Article.Add( articleEdit );
            //Какой-то неопознанный баг, пришлось костылить, простите меня(
            ModelState["Author"].Errors.Clear();
            if( !ModelState.IsValid ) {
                ViewBag.BlodID = blogId;
                return View( article );
            }
            db.SaveChanges();
            if( article == null )
                return View( "Error" );
            return RedirectToAction( "blog", "Blogs", new { id = article.Blogs.First().ID } );
        }

        [Authorize( Roles = "editor, admin" )]
        [HttpGet]
        [Route( "Delete/{id:int}" )]
        public ActionResult Delete( int id, int articlePosition )
        {
            Article article = db.Article.Where( p => id == p.ID ).FirstOrDefault();

            article.Likes_Authors.Clear();
            article.Dislikes_Authors.Clear();

            Person author = article.Author;
            author.Written_Articles.Remove( article );
            Blog[] blogs = article.Blogs.ToArray();
            int blogIdToRedirect = article.Blogs.First().ID;
            foreach( Blog blog in blogs ) {
                blog.Articles.Remove( article );
            }
            Comment[] comments = db.Comment.Where( p => p.Article.ID == article.ID ).ToArray();
            db.Article.Remove( article );
            foreach( Comment comment in comments ) {
                db.Comment.Remove( comment );
            }

            db.SaveChanges();
            if( articlePosition == 1 ) {
                return RedirectToAction( "blog", "Blogs", new { id = blogIdToRedirect } );
            } else {
                return RedirectToAction( "index", "home" );
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddComment( string comment, int id )
        {
            Article article = db.Article.Where( p => id == p.ID ).FirstOrDefault();
            if( comment != null && comment != "" ) {

                Person authorComment = db.Users.Where( p => User.Identity.Name == p.UserName ).FirstOrDefault();
                //TODO: delete this line

                Comment c = new Comment();

                c.Text = comment;
                c.Article = article;
                c.Author = authorComment;
                c.Create_Time = DateTime.Now;

                article.Comments.Add( c );

                db.SaveChanges();
                return RedirectToAction( "index", id );
            } else {
                ViewBag.EmptyComment = true;
                return View( "index", article );
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult DeleteComment( int idComment, int id )
        {
            Article article = db.Article.Where( p => id == p.ID ).FirstOrDefault();
            if( article == null )
                return View( "error" );
            Comment comment = db.Comment.Where( c => c.ID == idComment ).FirstOrDefault();
            if( comment == null )
                return View( "error" );
            if( comment.Author.UserName != User.Identity.Name && User.IsInRole( "admin" ) )
                return View( "error" );

            article.Comments.Remove( comment );
            db.Comment.Remove( comment );
            db.SaveChanges();
            return RedirectToAction( "index", id );
        }

        [Authorize]
        [HttpPost]
        [Route( "{id:int}/like" )]
        public JsonResult LikeArticle( HttpRequestMessage request, int id )
        {
            Article article = db.Article.Include( a => a.Author ).Where( p => p.ID == id ).FirstOrDefault();
            var um = new UserManager<Person>( new UserStore<Person>( db ) );
            var usr = um.FindByName( User.Identity.Name );
            int? likesCount = article.Likes_Count;
            if( likesCount == null ) {
                likesCount = 0;
            }
            if( !article.Dislikes_Authors.Contains( usr ) ) {
                if( article.Likes_Authors.Contains( usr ) ) {
                    --likesCount;
                    article.Likes_Authors.Remove( usr );
                } else {
                    ++likesCount;
                    article.Likes_Authors.Add( usr );
                }
                article.Likes_Count = likesCount;
                db.SaveChanges();
            };
            return Json( new { likesCount = likesCount } );
        }

        [Authorize]
        [HttpPost]
        [Route( "{id:int}/dislike" )]
        public JsonResult DislikeArticle( HttpRequestMessage request, int id )
        {
            Article article = db.Article.Include( a => a.Author ).Where( p => p.ID == id ).FirstOrDefault();
            var um = new UserManager<Person>( new UserStore<Person>( db ) );
            var usr = um.FindByName( User.Identity.Name );
            int? dislikesCount = article.Dislikes_Count;
            if( dislikesCount == null ) {
                dislikesCount = 0;
            }
            if( !article.Likes_Authors.Contains( usr ) ) {
                if( article.Dislikes_Authors.Contains( usr ) ) {
                    --dislikesCount;
                    article.Dislikes_Authors.Remove( usr );
                } else {
                    ++dislikesCount;
                    article.Dislikes_Authors.Add( usr );
                }
                article.Dislikes_Count = dislikesCount;
                db.SaveChanges();
            };
            return Json( new { dislikesCount = dislikesCount } );
        }
    }
}