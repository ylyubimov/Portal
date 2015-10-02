using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Portal.Models
{
    public class Comment
    {
        public int ID { get; set; }
        public string Text { get; set; }

        [ForeignKey("Person")]
        public int AuthorID { get; set; }
        public DateTime Create_time { get; set; }
        public DateTime Update_time { get; set; }
        // Автор комментария
        public virtual Person Person { get; set; }
    }
}