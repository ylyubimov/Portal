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
        public string Text { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<Picture> Pictures { get; set; }
        [Required]
        public virtual ICollection<Person> Authors { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Video>  Videos { get; set; }
        public virtual ICollection<Presentation> Presentations { get; set; }
    }
}