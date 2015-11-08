using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Portal.Models
{
    public class Article
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime? Date_of_Creation { get; set; }
        public int? Likes_Count { get; set; } 
        public int? Dislikes_Count { get; set; }
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
        [Required]
        public virtual Person Author { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Person> Likes_Authors { get; set; }
        public virtual ICollection<Person> Dislikes_Authors { get; set; }
    }
}