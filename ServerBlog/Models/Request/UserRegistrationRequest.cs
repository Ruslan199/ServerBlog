﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServerBlog.Models.Request
{
    public class UserRegistrationRequest
    {
        [Required]
        public string Login { get; set; }
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
