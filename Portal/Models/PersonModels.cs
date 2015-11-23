using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Portal.Models
{
    public class Person : IdentityUser
    {
        public string First_Name { get; set; }
        public string Second_Name { get; set; }
        public string Middle_Name { get; set; }
        public DateTime? Registration_Date { get; set; }
        public DateTime? Last_Date_Was_Online { get; set; }
        public string Person_Type { get; set; }
        [Required]
        public bool Exists { get; set; }
        public virtual Picture Picture { get; set; }
        public virtual ICollection<Article> Written_Articles { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
        // Кафедра есть и у студенета и у учителя
        public virtual Base_Company Base_Company { get; set; }

        // Поля студента
        public int? Grade_Number { get; set; }
        public string Grade_Name { get; set; }
        public int? Year_of_Graduating { get; set; }
        public virtual Faculty Faculty { get; set; }
        public virtual Group Group { get; set; }
        public virtual Base_Part Base_Part { get; set; }
        public virtual ICollection<Course> Subscribed_Courses { get; set; }
        public virtual ICollection<Program> Subscribed_Programs { get; set; }
        //

        // Поля учителя
        public string Position { get; set; }
        public string Working_Place { get; set; }
        public virtual ICollection<Course> Taught_Courses { get; set; }
        public virtual ICollection<Program> Taught_Programs { get; set; }
        //
        public static void DeleteUser(ApplicationDbContext db, string id)
        {
            Person person = db.Users.Where(p => p.Id == id).FirstOrDefault();
            person = db.Entry(person).Entity;
            Person def = db.Users.Where(p => p.UserName == "Deleted").FirstOrDefault();
            List<Article> articles = person.Written_Articles.ToList();
            foreach ( Article a in articles)
            {
                db.Entry(a).Entity.Author = def;
            }
            db.SaveChanges();
            foreach ( Comment c in db.Comment)
            {
                db.Entry(c).Entity.Author = def;
            }
            db.SaveChanges();
            if (person.Subscribed_Courses != null)
            {
                person.Subscribed_Courses.Clear();
            }
            db.SaveChanges();
            if (person.Subscribed_Programs != null)
            {
                person.Subscribed_Programs.Clear();
            }
            db.SaveChanges();
            if (person.Taught_Courses != null)
            {
                foreach( Course c in person.Taught_Courses )
                {
                    c.Teachers.Remove(person);
                    if( c.Teachers.Count() == 0)
                    {
                        c.Teachers.Add(def);
                    }
                    
                }
                person.Taught_Courses.Clear();
            }
            db.SaveChanges();
            if (person.Taught_Programs != null)
            {

                foreach (Program p in person.Taught_Programs)
                {
                    p.Teachers.Remove(person);
                    if (p.Teachers.Count() == 0)
                    {
                        p.Teachers.Add(def);
                    }
                }
                person.Taught_Programs.Clear();
            }
            db.SaveChanges();
            List<Blog> blogs;
            if (person.Blogs != null)
            {
                blogs = person.Blogs.ToList();
                foreach( Blog b in blogs)
                {
                    Blog bl = db.Entry(b).Entity;
                    bl.Author = def;
                }
            }
            db.SaveChanges();
            db.Users.Remove(person);
            db.SaveChanges();
        }
    }
   
}