using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Portal.Models
{
    public class CourseCreateEdit
    {
        public int ID { get; set; }
        [Required( ErrorMessage = "Введите имя курса" )]
        public string Name { get; set; }
        public DateTime? Date_and_Time { get; set; }
        public string Report_Type { get; set; }
        [Required( ErrorMessage = "Необходимо описание курса" )]
        public string Description { get; set; }
        [Range( 0, 1000, ErrorMessage = "Не может быть отрицательным" )]
        public int? Number_of_Hours { get; set; }
        [Range( 0, 1000, ErrorMessage = "Не может быть отрицательным" )]
        public int? Number_of_Classes { get; set; }
        public Person[] Teachers { get; set; }
        public bool[] Chosen_Teachers { get; set; }
        public Program[] Programs { get; set; }
        public bool[] Chosen_Programs { get; set; }
        public string Base_Part { get; set; }
        public Blog[] Blogs { get; set; }
        public bool[] Chosen_Blogs { get; set; }

    }
}