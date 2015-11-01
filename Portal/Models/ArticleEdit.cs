using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Portal.Models
{
    public class ArticleEdit
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Название обязательно")]
        public string Name { get; set; }
        public string Text { get; set; }
        public virtual string Blogs { get; set; }
        public virtual ICollection<Picture> Pictures { get; set; }
        public virtual ICollection<Video> Videos { get; set; }
        public virtual ICollection<Presentation> Presentations { get; set; }
    }
}