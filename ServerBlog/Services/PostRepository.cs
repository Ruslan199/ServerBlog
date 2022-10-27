using ServerBlog.Models;
using ServerBlog.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerBlog.Services
{
    public class PostRepository : EntityBaseRepository<Post>, IPostRepository
    {
        public PostRepository(PostDBContext context) : base(context) { }
    }
}
