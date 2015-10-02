using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public class Presentation
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }

    }
    public class Picture
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
    }
    public class Video
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
    }
}