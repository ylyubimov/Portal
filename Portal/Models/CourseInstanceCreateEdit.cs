using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Portal.Models
{
    public class CourseInstanceCreateEdit
    {
        public int ID { get; set; }
        [Required( ErrorMessage = "Необходимо указать год проведения" )]
        public int Year { get; set; }
        [Required( ErrorMessage = "Необходимо указать курс" )]
        public virtual Course BaseCourse { get; set; }
        public string Place { get; set; }
        public string AdditionalDescription { get; set; }
        public DateTime? Report_Date { get; set; }
        public Person[] Students { get; set; }
        public bool[] Chosen_Students { get; set; }

    }
}