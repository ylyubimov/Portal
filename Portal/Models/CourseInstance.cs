using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Portal.Models
{
    public class CourseInstance
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public virtual Course BaseCourse { get; set; }
        public string Place { get; set; }
        public string AdditionalDescription { get; set; }
        public DateTime? Report_Date { get; set; }
        public virtual ICollection<Person> Students { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
    }
}