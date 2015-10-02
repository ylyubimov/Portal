using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public class Group
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Faculty FacultyID { get; set; }
        public int Grade { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual Faculty Faculty { get; set; }
    }
    public class Faculty
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
    }
    public class Base_Company
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Faculty> Faculties { get; set; }
        public virtual ICollection<Base_Part> Base_Parts { get; set; }
    }
    public class Base_Part
    {
        public int ID { get; set; }
        public int Base_CompanyID { get; set; }
        public string Name { get; set; }
        public virtual Base_Company Base_Сompany { get; set; }
    }
}