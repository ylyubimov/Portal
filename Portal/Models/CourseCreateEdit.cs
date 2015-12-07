using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Portal.Models
{
    public class CourseCreateEdit
    {
        public string Name { get; set; }
        //public string Present { get; set; }
        public DateTime? Date_and_Time { get; set; }
        public string Report_Type { get; set; }
        public DateTime? Report_Date { get; set; }
        public string Place { get; set; }
        public string Description { get; set; }
        public int? Number_of_Hours { get; set; }
        public int? Number_of_Classes { get; set; }

        public ICollection<Tuple<Person,bool>> Students { get; set; }
        public ICollection<Tuple<Person,bool>> Teachers { get; set; }
  
  
    }
}