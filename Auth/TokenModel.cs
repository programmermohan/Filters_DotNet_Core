using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore_Filters.Auth
{
    public class TokenModel
    {
        public string Access_Token { get; set; }

        public string Refresh_Token { get; set; }
    }
}
