using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore_Filters.Filters.ResultFilters
{
    public class ResultAsyncFilterExample : Attribute, IAsyncResultFilter
    {
        /*Have to learn and understand 
         * what is the use of Result filter
         * what is the benefits of using it, right now just logging the 
         */
        private readonly ILogger<ResultAsyncFilterExample> _logger;
        public ResultAsyncFilterExample(ILogger<ResultAsyncFilterExample> logger)
        {
            this._logger = logger;
        }
        public Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            try
            {
                var username = context.HttpContext.User.Identity.Name;
                var trace = context.ActionDescriptor.RouteValues; //dictionary
                string Controller = trace["Controller"];
                string ActionName = trace["Action"];

                string StrAppendText = "User: " + username + "\n" + "Controller: " + Controller + "\n" + "Action: " + ActionName + "\n" +
                             " -> " + context.ActionDescriptor.ToString() + " -> On Result Executed \t- " + DateTime.Now.ToString() + "\n";

                _logger.LogInformation(StrAppendText);

                return Task.CompletedTask;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
