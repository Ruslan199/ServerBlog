﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerBlog.Models.Response
{
    public class AllPostUser : BaseResponse
    {
        public IEnumerable<Post> Posts { get; set; }
    }
}
