using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Portal.Models
{
    public class Group
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public int Grade { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual Faculty Faculty { get; set; }
    }
    public class Faculty
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
    }
    public class Base_Company
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public virtual Faculty Faculty { get; set; }
        public virtual ICollection<Base_Part> Base_Parts { get; set; }
    }
    public class Base_Part
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public virtual Base_Company Base_Сompany { get; set; }
    }
}