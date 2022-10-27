using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServerBlog.Models
{
    public class User
    {

        [Key]
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Login { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}
