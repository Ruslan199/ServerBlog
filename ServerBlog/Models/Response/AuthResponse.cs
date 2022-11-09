using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerBlog.Models.Response
{
    public class AuthResponse : BaseResponse
    {
        public string Token { get; set; }

        public string UserId { get; set; }
    }
}
