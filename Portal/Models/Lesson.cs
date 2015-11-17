using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Portal.Models
{
    public class Lesson
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Name;
        public string Description;
        public string Links;
    }
}