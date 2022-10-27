using ServerBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerBlog.Services.Abstractions
{
    public interface IPostRepository : IEntityBaseRepository<Post>
    {
    }
}
