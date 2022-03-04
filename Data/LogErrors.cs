using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore_Filters.Data
{
    public class LogErrors
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string IPAddress { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string Message { get; set; }
        public string ErrorMessage { get; set; }
        public string DetailMessage { get; set; }
    }
}
