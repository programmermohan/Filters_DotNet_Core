using DotNetCore_Filters.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore_Filters.Filters.ActionFilters
{
    public class ActionFilterWithCustom : Attribute, IActionFilter
    {
        private Microsoft.AspNetCore.Hosting.IWebHostEnvironment _env;
        public ActionFilterWithCustom(Microsoft.AspNetCore.Hosting.IWebHostEnvironment env)
        {
            _env = env;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            try
            {
                var webRoot = _env.ContentRootPath;
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

                var file = System.IO.Path.Combine(webRoot + "\\LogInfo", "LogDetails" + DateTime.Today.ToString("ddMMyyyy") + ".txt");
                System.IO.File.AppendAllText(file, StrAppendText);
                //_logger.Info(StrAppendText);
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message);
#endif
                // _logger.Error(ex, ex.Message);
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                var webRoot = _env.ContentRootPath;
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

                //_logger.Info(StrAppendText);
                var file = System.IO.Path.Combine(webRoot + "\\LogInfo", "LogDetails" + DateTime.Today.ToString("ddMMyyyy") + ".txt");
                System.IO.File.AppendAllText(file, StrAppendText);
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message);
#endif
                //_logger.Error(ex, ex.Message);
            }
        }
    }
}
