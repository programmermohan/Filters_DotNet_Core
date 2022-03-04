using DotNetCore_Filters.Auth;
using DotNetCore_Filters.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore_Filters.Services
{
    public interface IUserLoginManager
    {
        long UpdateLoginHistory(string IP, LoginUser userModel, string AccessToken, string RefreshToken);
        bool UserLoggedInDifferentIP(string IP, LoginUser userModel);
        bool DifferentIPLoggedIn(string IP, LoginUser userModel, string AccessToken, string RefreshToken);
        long UpdateRefreshToken(string IP, LoginUser userModel, string AccessToken, string RefreshToken);
        long RevokeRefreshToken(string IP, LoginUser user, string AccessToken, string RefreshToken);
    }
}
