using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Portal.Models
{
    public class Presentation
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        [Required]
        public string URL { get; set; }
        public DateTime? Date_Of_Uploading { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<Article> Articles { get; set; }

    }
    public class Picture
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        [Required]
        public string URL { get; set; }
        public DateTime? Date_Of_Uploading { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<Article> Articles { get; set; }

    }

    public class Document
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [Required]
        public string URL { get; set; }
        public DateTime? Date_Of_Uploading { get; set; }
        public virtual Person Person { get; set; }
        public virtual ICollection<Article> Articles { get; set; }

    }
    public class Video
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        [Required]
        public string URL { get; set; }
        public DateTime? Date_Of_Uploading { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
    }
  /*  public class Document
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        [Required]
        public string URL { get; set; }
        public DateTime? Date_Of_Uploading { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
    }*/
}