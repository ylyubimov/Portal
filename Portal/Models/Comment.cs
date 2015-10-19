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
        [Key]
        public int ID { get; set; }
        [Required]
        public string Text { get; set; }
        public DateTime? Create_Time { get; set; }
        public DateTime? Update_Time { get; set; }
        [Required]
        public virtual Person Author { get; set; }
        public virtual Article Article { get; set; }
    }
}