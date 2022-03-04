using DotNetCore_Filters.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DotNetCore_Filters.Filters.ActionFilters
{
    /*if I add attribute then we will be abale to 
     * configure our action method at controller level or action level*/
    public class ActionFilterExample : Attribute, IActionFilter
    {
        private readonly ILogFactory _logFactory;
        public ActionFilterExample(ILogFactory logFactory)
        {
            this._logFactory = logFactory;
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------//
        public void OnActionExecuted(ActionExecutedContext context)
        {
            try
            {
                //get the username from Identity
                var username = context.HttpContext.User.Identity.Name;
                var trace = context.ActionDescriptor.RouteValues; //dictionary
                string Controller = trace["Controller"];
                string ActionName = trace["Action"];

                var HostIP = context.HttpContext.Request.Host.Value;

                string Message = "\n" + context.Controller.ToString() +
                         " -> " + context.ActionDescriptor.ToString() + " -> On Action Executed \t- " + DateTime.Now.ToString() + "\n";

                bool Saved = _logFactory.SaveLogFromAction(username, HostIP, Controller, ActionName, Message);

                ////From MSDN
                //var data = MethodBase.GetCurrentMethod();
                //var path = context.HttpContext.Request.Path;
                //Debug.WriteLine(data + " ", path);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------//
        public void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                //get the username from Identity
                var username = context.HttpContext.User.Identity.Name;
                var trace = context.ActionDescriptor.RouteValues; //dictionary
                string Controller = trace["Controller"];
                string ActionName = trace["Action"];

                var HostIP = context.HttpContext.Request.Host.Value;

                string Message = "\n" + context.Controller.ToString() +
                         " -> " + context.ActionDescriptor.ToString() + " -> On Action Executing \t- " + DateTime.Now.ToString() + "\n";

                bool Saved = _logFactory.SaveLogFromAction(username, HostIP, Controller, ActionName, Message);

                //From MSDN
                //var data = MethodBase.GetCurrentMethod();
                //var path = context.HttpContext.Request.Path;
                //Debug.WriteLine(data + " ", path);
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
