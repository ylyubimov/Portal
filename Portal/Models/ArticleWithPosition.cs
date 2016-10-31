using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Portal.Models
{
    public class ArticleWithPosition
    {
        [Required]
        public Article ArticleInfo { get; set; }
        public int? Position;
    }

    public class ArticleListWithPosition
    {
        [Required]
        public Article[] ArticleInfo { get; set; }
        public int? Position;
    }
}