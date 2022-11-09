using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServerBlog.Models.Entities
{
    public class PostsAnotherUsers
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Content { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public DateTime CreatedOn { get; set; }

        [Required]
        [MaxLength(100)]
        public Guid PostId { get; set; }
    }
}
