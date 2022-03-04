using DotNetCore_Filters.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore_Filters.Data
{
    public class LogFactory : ILogFactory
    {
        private readonly DatabaseContext _databaseContext;

        public LogFactory(DatabaseContext databaseContext)
        {
            this._databaseContext = databaseContext;
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------//
        public bool SaveLogFromAction(string Username, string IPAddress, string ControllerName, string ActionName, string Message)
        {
            try
            {
                User user = _databaseContext.Users.FirstOrDefault(a => a.UserName == Username);
                if (user != null)
                {
                    LogDetails logDetails = new LogDetails()
                    {
                        UserId = user.Id,
                        ControllerName = ControllerName,
                        ActionName = ActionName,
                        IPAddress = IPAddress,
                        Message = Message
                    };

                    _databaseContext.LogDetails.Add(logDetails);

                    _databaseContext.SaveChanges();
                }
                return true;
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
        public bool SaveLogFromException(string Username, string IPAddress, string ControllerName, string ActionName, string Messages, string ErrorMessage, string DetailError)
        {
            try
            {
                bool IsSaved = true;
                User user = _databaseContext.Users.FirstOrDefault(a => a.UserName == Username);
                if (user != null)
                {
                    LogErrors logErrors = new LogErrors()
                    {
                        UserId = user.Id,
                        IPAddress = IPAddress,
                        ActionName = ActionName,
                        ControllerName = ControllerName,
                        ErrorMessage = ErrorMessage,
                        Message = Messages,
                        DetailMessage = DetailError
                    };
                    _databaseContext.LogErrors.Add(logErrors);
                    _databaseContext.SaveChanges();
                }
                else
                    IsSaved = false;

                return IsSaved;
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
    }
}
