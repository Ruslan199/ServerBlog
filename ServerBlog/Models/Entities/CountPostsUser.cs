using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerBlog.Models.Entities
{
    public class CountPostsUser
    {
        public string UserName { get; set; }

        public int CountPost { get; set; }
    }
}
