using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore_Filters.Services
{
    public interface ILogFactory
    {
        bool SaveLogFromAction(string Username, string IPAddress, string ControllerName, string ActionName, string Messages);

        bool SaveLogFromException(string Username, string IPAddress, string ControllerName, string ActionName, string Messages, string ErrorMessage, string DetailError);
    }
}
