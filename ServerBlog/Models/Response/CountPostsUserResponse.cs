using ServerBlog.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerBlog.Models.Response
{
    public class CountPostsUserResponse : BaseResponse
    {
        public List<CountPostsUser> AllPosts { get; set; }
    }
}
