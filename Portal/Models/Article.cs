using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public class Article
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime Date_of_creation { get; set; }
        public int Likes_count { get; set; }
        public int Dislikes_count { get; set; }
        public string Text { get; set; }
        public virtual ICollection<Picture> Pictures { get; set; }
        // Авторы
        public virtual ICollection<Person> Persons { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Video>  Videos { get; set; }
    }
}