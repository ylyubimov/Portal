﻿using System;
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
        [Required( ErrorMessage = "Заполните название урока" )]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Links { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
    }
}