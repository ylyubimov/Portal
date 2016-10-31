using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Portal.Models
{
    public class Blog
    {
        [Key]
        public int ID { get; set; }
        public int? Likes_Count { get; set; }
        public int? Dislikes_Count { get; set; }
        [Required( ErrorMessage = "Заполните название блога" )]
        [StringLength( 127, MinimumLength = 1, ErrorMessage = "Длина названия не должна превышать 127 символов" )]
        public string Name { get; set; }
        public int? Rating { get; set; }
        [Required]
        public virtual Person Author { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
    }
}