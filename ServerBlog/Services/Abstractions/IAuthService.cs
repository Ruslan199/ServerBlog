using ServerBlog.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerBlog.Services.Abstractions
{
    public interface IAuthService
    {
        AuthData GetAuthData(Guid id);
    }
}
