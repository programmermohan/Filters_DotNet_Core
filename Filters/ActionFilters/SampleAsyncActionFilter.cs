using DotNetCore_Filters.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore_Filters.Filters.ActionFilters
{
    public class SampleAsyncActionFilter : IAsyncActionFilter
    {
        private readonly ILogFactory _logFactory;

        public SampleAsyncActionFilter(ILogFactory logFactory)
        {
            this._logFactory = logFactory;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                //Before action executes
                var username = context.HttpContext.User.Identity.Name;
                var trace = context.ActionDescriptor.RouteValues; //dictionary
                string Controller = trace["Controller"];
                string ActionName = trace["Action"];

                var HostIP = context.HttpContext.Request.Host.Value;

                string BeforeExecution = "\n" + context.Controller.ToString() +
                         " -> " + context.ActionDescriptor.ToString() + " -> On Action Executed \t- " + DateTime.Now.ToString() + "\n";

                bool SaveBeforeExecution = _logFactory.SaveLogFromAction(username, HostIP, Controller, ActionName, BeforeExecution);

                var result = await next();

                //After action executes
                string AfterExecution = "\n" + context.Controller.ToString() +
                         " -> " + context.ActionDescriptor.ToString() + " -> After Action Executes \t- " + DateTime.Now.ToString() + "\n";

                bool SaveAfterExecution = _logFactory.SaveLogFromAction(username, HostIP, Controller, ActionName, AfterExecution);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
