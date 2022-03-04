using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore_Filters.Filters.ResourceFilters
{
    public class ResourceFilterSample : Attribute, IResourceFilter
    {
        private readonly string[] _headers;
        public ResourceFilterSample(params string[] headers)
        {
            _headers = headers;
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------//
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            try
            {
                var username = context.HttpContext.User.Identity.Name;
                var trace = context.ActionDescriptor.RouteValues; //dictionary
                string Controller = trace["Controller"];
                string ActionName = trace["Action"];

                var HostIP = context.HttpContext.Request.Host.Value;

                string AfterExecution = "\n" + Controller.ToString() +
                         " -> " + context.ActionDescriptor.ToString() + " -> On Resource Executed \t- " + DateTime.Now.ToString() + "\n";
            }
            catch (Exception)
            {
                throw;
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------//
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            try
            {
                if (_headers == null) return;

                if (!_headers.All(m => context.HttpContext.Request.Headers.ContainsKey(m)))
                {
                    context.Result = new JsonResult(new { Error = "Headers Missing" }) { StatusCode = 400 };
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------//
    }
}
