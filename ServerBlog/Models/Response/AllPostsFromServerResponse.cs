using ServerBlog.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerBlog.Models.Response
{
    public class AllPostsFromServerResponse : BaseResponse
    {
        public List<AllPostFromBlog> Posts { get; set; }
    }
}
