using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Portal.Models
{
    public class Blog
    {
        [Key]
        public int ID { get; set; }
        public int? Likes_Count { get; set; }
        public int? Dislikes_Count { get; set; }
        [Required]
        public string Name { get; set; }
        public int? Rating { get; set; }
        [Required]
        public virtual ICollection<Person> Authors { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
    }
}