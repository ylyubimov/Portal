﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Portal.Models
{
    public partial class ApplicationUser : IdentityUser
    {
        public string First_Name { get; set; }
        public string Second_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Phone { get; set; }
        public DateTime? Registration_Date { get; set; }
        public DateTime? Last_Date_Was_Online { get; set; }
        public string Email { get; set; }
        public virtual Picture Picture { get; set; }
        public virtual ICollection<Course> Subscribed_Courses { get; set; }
        public virtual ICollection<Article> Written_Articles { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }

    }

    public class Student : ApplicationUser
    {
        public int? Grade_Number { get; set; }
        public string Grade_Name { get; set; }
        public int? Year_of_Graduating { get; set; }
        public virtual Faculty Faculty { get; set; }
        public virtual Group Group { get; set; }
        public virtual Base_Company Base_Company { get; set; }
        public virtual Base_Part Base_Part { get; set; }
        public virtual ICollection<Course> Necessary_Courses { get; set; }

    }
    public class Teacher : ApplicationUser
    {
        public string Position { get; set; }
        public string Working_Place { get; set; }
        public virtual Base_Company Base_Company { get; set; }
        public virtual ICollection<Course> Taught_Courses { get; set; }
    }
}