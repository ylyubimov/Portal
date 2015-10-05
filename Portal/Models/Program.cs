using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public class Program
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Grade { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<Teacher> Teachers { get; set; }
        public virtual ICollection<Person> Subscribers { get; set; }
        public virtual ICollection<Course> Courses { get; set; }

        //test
        public Program(int _id)
        {
            ID = _id;
            Courses = new Course[3];
        }
    }
}