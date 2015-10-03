using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public class Person
    {
        public int ID { get; set; }
        public string Person_Type { get; set; }
        public string First_Name { get; set; }
        public string SecondName { get; set; }
        public string MiddleName { get; set; }
        public string Phone { get; set; }
        public DateTime Registration_Date { get; set; }
        public DateTime Last_Date_Was_Online { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public virtual Picture Picture { get; set; }
       
    }

    public class Student : Person
    {
        public int GradeNumber { get; set; }
        public string GradeName { get; set; }
        public int Year_of_Graduating { get; set; }
        public virtual Faculty Faculty { get; set; }
        public virtual Group Group { get; set; }
        public virtual Base_Company Base_Company { get; set; }
        public virtual Base_Part Base_Part { get; set; }

    }
    public class Teacher : Person
    {
        public string Position { get; set; }
        public string Company { get; set; }
        public string Working_Place { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}