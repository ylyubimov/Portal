using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public class Blog
    {
        public int ID { get; set; }
        public int Likes_count { get; set; }
        public int Dislikes_count { get; set; }
        public string Name { get; set; }
        public int Rating { get; set; }
        public virtual ICollection<Person> Authors { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
    }
}