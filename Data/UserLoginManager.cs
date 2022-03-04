using DotNetCore_Filters.Auth;
using DotNetCore_Filters.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore_Filters.Data
{
    public class UserLoginManager : IUserLoginManager
    {

        private readonly DatabaseContext _DbContext;
        private readonly IConfiguration _configuration;

        public UserLoginManager(DatabaseContext DbContext, IConfiguration configuration)
        {
            _DbContext = DbContext;
            _configuration = configuration;
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------//
        public bool DifferentIPLoggedIn(string IP, LoginUser userModel, string AccessToken, string RefreshToken)
        {
            try
            {
                bool IsAnotherUserLogin = false;

                TokenHistory tokenHistory = _DbContext.TokenHistories.
                    Where(a => a.User.UserName == userModel.UserName && a.AccessToken != AccessToken && a.RefreshToken != RefreshToken).
                    OrderByDescending(a => a.AccessToken_Expiry).FirstOrDefault();

                if (tokenHistory != null)
                {
                    if (tokenHistory.IsAcess_TokenAlive == false)
                    {
                        TimeSpan timeDiff = DateTime.Now.Subtract(tokenHistory.AccessToken_Expiry);
                        if (timeDiff.TotalMinutes < 20)
                        {
                            IsAnotherUserLogin = true;
                        }
                    }
                }
                return IsAnotherUserLogin;
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message);
#endif
                throw;
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------//
        public long RevokeRefreshToken(string IP, LoginUser user, string AccessToken, string RefreshToken)
        {
            try
            {
                TokenHistory tokenHistory = _DbContext.TokenHistories.Where(a => a.User.UserName == user.UserName && a.IPAddress == IP)
                    .OrderByDescending(a => a.AccessToken_Expiry).FirstOrDefault();
                if (tokenHistory == null)
                    return 0;
                else
                {
                    tokenHistory.IsActive = false;
                    tokenHistory.IsAcess_TokenAlive = true;
                    tokenHistory.IsRefreshToken_Alive = true;

                    _DbContext.SaveChanges();

                    return tokenHistory.Id;
                }

            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message);
#endif
                throw;
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------//
        public long UpdateLoginHistory(string IP, LoginUser userModel, string AccessToken, string RefreshToken)
        {
            try
            {
                //disable other IPs and tokens for this user
                User user = _DbContext.Users.FirstOrDefault(a => a.UserName == userModel.UserName);
                List<TokenHistory> lstTokenHistory = _DbContext.TokenHistories.Where(a => a.User.UserName == userModel.UserName).ToList();
                lstTokenHistory.ForEach(a =>
                {
                    a.IsActive = false; a.IsAcess_TokenAlive = true; a.IsRefreshToken_Alive = true;
                });
                _DbContext.SaveChanges();

                TokenHistory tokenHistory = _DbContext.TokenHistories.FirstOrDefault(a => a.User.UserName == userModel.UserName && a.IPAddress == IP);
                if (tokenHistory != null)
                {
                    tokenHistory.IPAddress = IP;
                    tokenHistory.AccessToken = AccessToken;
                    tokenHistory.RefreshToken = RefreshToken;
                    tokenHistory.IsActive = true;
                    tokenHistory.IsAcess_TokenAlive = false;
                    tokenHistory.IsRefreshToken_Alive = false;
                    tokenHistory.ModifyTokenDtTm = DateTime.Now;
                    tokenHistory.AccessToken_Expiry = DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JWT:AccessTokenExpire"]));
                    tokenHistory.RefreshToken_Expiry = DateTime.Now.AddHours(Convert.ToInt32(_configuration["JWT:RefreshTokenExpire"]));

                    _DbContext.SaveChanges();

                    return tokenHistory.Id;
                }
                else
                {
                    tokenHistory = new TokenHistory()
                    {
                        UserId = user.Id,
                        IPAddress = IP,
                        AccessToken = AccessToken,
                        RefreshToken = RefreshToken,
                        IsActive = true,
                        IsAcess_TokenAlive = false,
                        IsRefreshToken_Alive = false,
                        LoginDtTm = DateTime.Now,
                        ModifyTokenDtTm = DateTime.Now,
                        AccessToken_Expiry = DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JWT:AccessTokenExpire"])),
                        RefreshToken_Expiry = DateTime.Now.AddHours(Convert.ToInt32(_configuration["JWT:RefreshTokenExpire"]))
                    };

                    _DbContext.TokenHistories.Add(tokenHistory);
                    _DbContext.SaveChanges();

                    return tokenHistory.Id;
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message);
#endif
                throw;
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------//
        public long UpdateRefreshToken(string IP, LoginUser userModel, string AccessToken, string RefreshToken)
        {
            try
            {
                TokenHistory tokenHistory = _DbContext.TokenHistories.Where(a => a.User.UserName == userModel.UserName && a.IPAddress == IP)
                    .OrderByDescending(a => a.AccessToken_Expiry).FirstOrDefault();

                if (tokenHistory == null)
                    return 0;
                else
                {
                    tokenHistory.AccessToken = AccessToken;
                    tokenHistory.RefreshToken = RefreshToken;
                    tokenHistory.IsActive = true;
                    tokenHistory.ModifyTokenDtTm = DateTime.Now;
                    tokenHistory.IsAcess_TokenAlive = false;
                    tokenHistory.IsRefreshToken_Alive = false;
                    tokenHistory.AccessToken_Expiry = DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JWT:AccessTokenExpire"]));
                    tokenHistory.RefreshToken_Expiry = DateTime.Now.AddHours(Convert.ToInt32(_configuration["JWT:RefreshTokenExpire"]));

                    _DbContext.SaveChanges();
                    return tokenHistory.Id;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------//
        public bool UserLoggedInDifferentIP(string IP, LoginUser userModel)
        {
            try
            {
                bool IsUserLogin = false;

                TokenHistory tokenHistory = _DbContext.TokenHistories.Where(a => a.User.UserName == userModel.UserName).OrderByDescending(a => a.AccessToken_Expiry).FirstOrDefault();

                if (tokenHistory != null)
                {
                    if (tokenHistory.IsAcess_TokenAlive == false)
                    {
                        TimeSpan timeDiff = DateTime.Now.Subtract(tokenHistory.AccessToken_Expiry);

                        if (timeDiff.TotalMinutes < 20)
                        {
                            IsUserLogin = true;
                        }
                    }
                }

                return IsUserLogin;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------//
    }
}
