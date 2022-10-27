using ServerBlog.Models;
using ServerBlog.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerBlog.Services
{
    public class UserRepository : EntityBaseRepository<User>, IUserRepository
    {
        public UserRepository(PostDBContext context) : base(context) { }
    }
}
