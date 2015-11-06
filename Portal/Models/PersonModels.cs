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
        public string Phone { get; set; }
        public DateTime? Registration_Date { get; set; }
        public DateTime? Last_Date_Was_Online { get; set; }
        public string Person_Type { get; set; }
        [Required]
        public string Email { get; set; }
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
    }
}