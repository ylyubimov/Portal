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
        public string Name { get; set; }
        public DateTime? Date_and_Time { get; set; }
        public string Report_Type { get; set; }
        public DateTime? Report_Date { get; set; }
        public string Place { get; set; }
        public string Description { get; set; }
        public int? Number_of_Hours { get; set; }
        public int? Number_of_Classes { get; set; }

        public Person[] Students { get; set; }
        public bool[] Chosen_Students { get; set; }
        public Person[] Teachers { get; set; }
        public bool[] Chosen_Teachers { get; set;}
        public Program[] Programs { get; set; }
        public bool[] Chosen_Programs { get; set; }
        public string Base_Part { get; set; }
                
    }
}