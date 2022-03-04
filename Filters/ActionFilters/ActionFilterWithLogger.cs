using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore_Filters.Filters.ActionFilters
{
    public class ActionFilterWithLogger : Attribute, IActionFilter
    {
        private readonly ILogger _logger;
        public ActionFilterWithLogger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("ActionFilterWithLogger");
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------//
        public void OnActionExecuted(ActionExecutedContext context)
        {
            try
            {
                var username = context.HttpContext.User.Identity.Name;
                var trace = context.ActionDescriptor.RouteValues; //dictionary
                string Controller = trace["Controller"];
                string ActionName = trace["Action"];

                var HostIP = context.HttpContext.Request.Host.Value;

                string AfterExecution = "\n" + context.Controller.ToString() +
                         " -> " + context.ActionDescriptor.ToString() + " -> On Action Executed \t- " + DateTime.Now.ToString() + "\n";

                string StrAppendText = DateTime.Now.ToString() + "\n" + "IPAddress: " + HostIP + "\n" + "UserName: " + username +
                    "\n" + "Controller: " + Controller + "\n" + "ActionName: " + ActionName + "\n"
                    + "AfterExecution: " + AfterExecution + "";

                _logger.LogInformation(StrAppendText);
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message);
#endif
                 _logger.LogError(ex, ex.Message);
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------//
        public void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                var username = context.HttpContext.User.Identity.Name;
                var trace = context.ActionDescriptor.RouteValues; //dictionary
                string Controller = trace["Controller"];
                string ActionName = trace["Action"];

                var HostIP = context.HttpContext.Request.Host.Value;

                string BeforeExecution = "\n" + context.Controller.ToString() +
                         " -> " + context.ActionDescriptor.ToString() + " -> On Action Executing \t- " + DateTime.Now.ToString() + "\n";

                string StrAppendText = DateTime.Now.ToString() + "\n" + "IPAddress: " + HostIP + "\n" + "UserName: " + username +
                    "\n" + "Controller: " + Controller + "\n" + "ActionName: " + ActionName + "\n"
                    + "BeforeExecution: " + BeforeExecution + "";

                _logger.LogInformation(StrAppendText);
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message);
#endif
                _logger.LogError(ex, ex.Message);
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------//
    }
}
