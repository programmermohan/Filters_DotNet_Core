using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore_Filters.Services
{
    public interface IUserLoginDtls
    {
        bool SaveUserLogin(string UserName, string IPAddress, bool IsSuccess);
    }
}
