using DotNetCore_Filters.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore_Filters.Data
{
    public class UserLoginDtls : IUserLoginDtls
    {
        private readonly DatabaseContext _databaseContext;

        public UserLoginDtls(DatabaseContext databaseContext)
        {
            this._databaseContext = databaseContext;
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------//
        public bool SaveUserLogin(string UserName, string IPAddress, bool IsSuccess)
        {
            try
            {
                bool SavedData = true;
                User user = _databaseContext.Users.FirstOrDefault(a => a.UserName == UserName);

                if (user != null)
                {
                    UserLogin userLogin = new UserLogin()
                    {
                        IPAddress = IPAddress,
                        IsSuccess = IsSuccess,
                        LoginDate = DateTime.Now,
                        UserId = user.Id
                    };

                    _databaseContext.UserLogins.Add(userLogin);
                    _databaseContext.SaveChanges();
                }
                else
                    SavedData = false;
                return SavedData;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------//
    }
}
