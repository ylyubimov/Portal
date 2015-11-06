using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Portal.Models
{
    public class Program
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public int Grade { get; set; }
        public virtual ICollection<Person> Students { get; set; }
        public virtual ICollection<Person> Teachers { get; set; }
        [Required]
        public virtual ICollection<Course> Courses { get; set; }
    }
}