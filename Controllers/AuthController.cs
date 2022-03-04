using DotNetCore_Filters.Auth;
using DotNetCore_Filters.Data;
using DotNetCore_Filters.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DotNetCore_Filters.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DatabaseContext _DbContext;
        private readonly ITokenManager _tokenManager;
        private readonly IUserLoginManager _userLogManager;
        private readonly IUserLoginDtls _userLoginDtls;

        public AuthController(DatabaseContext DbContext, ITokenManager tokenManager, IUserLoginManager userLogManager, IUserLoginDtls userLoginDtls)
        {            
            this._DbContext = DbContext ?? throw new ArgumentNullException(nameof(DbContext));
            this._tokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));
            this._userLogManager = userLogManager ?? throw new ArgumentNullException(nameof(userLogManager));
            this._userLoginDtls = userLoginDtls ?? throw new ArgumentNullException(nameof(userLoginDtls));
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------//

        [HttpPost, Route("login")]
        public IActionResult Login([FromBody] LoginUser UserModel)
        {
            if (UserModel == null)
            {
                return BadRequest("Invalid client request");
            }

            string IP = HttpContext.Request.Host.Value;
            var user = _DbContext.Users.FirstOrDefault(u => (u.UserName == UserModel.UserName) && (u.Password == UserModel.Password));
            var role = _DbContext.Users.Where(u => (u.UserName == UserModel.UserName) && (u.Password == UserModel.Password)).Select(a => a.Role).FirstOrDefault();

            if (user == null)
            {
                return Unauthorized();
            }
            var claims = new List<Claim>
            {
              new Claim(ClaimTypes.Name, UserModel.UserName),
              new Claim(ClaimTypes.Role, role.RoleName)
            };

            bool IsAlreadyLoggedIn = _userLogManager.UserLoggedInDifferentIP(IP, UserModel);
            if (IsAlreadyLoggedIn)
            {
                bool SavedLoginDtls = _userLoginDtls.SaveUserLogin(user.UserName, IP, false);
                return Ok("user is already loggedin");
            }

            var accessToken = _tokenManager.GenerateAccessToken(claims);
            var refreshToken = _tokenManager.GenerateRefreshToken();

            //update the database login history
            long Id = _userLogManager.UpdateLoginHistory(IP, UserModel, accessToken, refreshToken);
            bool SavedLogin = _userLoginDtls.SaveUserLogin(user.UserName, IP, true);

            return Ok(new
            {
                Token = accessToken,
                RefreshToken = refreshToken
            });
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------//

    }
}
