﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Portal.Models
{
    public class CourseEdit
    {
        public string Name { get; set; }
        public string Present { get; set; }
        public DateTime? Date_and_Time { get; set; }
        public string Report_type { get; set; }
        public string Place { get; set; }
        public string Description { get; set; }
        public int? Number_of_Hours { get; set; }
        public int? Number_of_Classes { get; set; }
        public virtual ICollection<Person> Subscribers { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        [Required]
        public virtual ICollection<Teacher> Teachers { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<Program> Programs { get; set; }
        public virtual Faculty Faculty { get; set; }
        public virtual ICollection<Presentation> Presentations { get; set; }
        public virtual ICollection<Video> Videos { get; set; }
        public virtual ICollection<Picture> Pictures { get; set; }
    }
}