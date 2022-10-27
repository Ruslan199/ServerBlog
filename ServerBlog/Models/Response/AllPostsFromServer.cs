using ServerBlog.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerBlog.Models.Response
{
    public class AllPostsFromServer
    {
        public List<AllPosts> AllPosts { get; set; }
    }
}
