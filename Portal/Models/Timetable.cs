using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Portal.Models
{
    public class Timetable
    {
        [Key]
        public int ID { get; set; }
                
        public virtual ICollection<LessonShedule> Lessons { get; set; }
    }

    public class LessonShedule
    {
        [Key]
        public int ID { get; set; }

        public string Lesson_Type { get; set; }
        public DateTime Start_Time { get; set; }
        public DateTime End_Time { get; set; }
        public string Place { get; set; }
        public string Comment { get; set; }
    }
}