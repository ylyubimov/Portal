using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Portal.Models
{
    public class Course
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public string Present { get; set; }
        public DateTime? Date_and_Time { get; set; }
        public string Report_type { get; set; }
        public string Place { get; set; }
        public string Description { get; set; }
        public DateTime? Report_Date { get; set; }
        public int? Number_of_Hours { get; set; }
        public int? Number_of_Classes { get; set; }
        public virtual ICollection<Person> Students { get; set; }
        [Required]
        public virtual ICollection<Person> Teachers { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<Program> Programs { get; set; }
        public virtual Faculty Faculty { get; set; }
        bool deleted = false;
        public virtual ICollection<Lesson> Lessons { get; set; }
    }
}