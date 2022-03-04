using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore_Filters.Filters.ResourceFilters
{
    public class ResourceFilterAsyncSample : Attribute, IAsyncResourceFilter
    {
        private readonly string[] _headers;

        public ResourceFilterAsyncSample(params string[] headers)
        {
            _headers = headers;
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            if (_headers == null) return;

            if (!_headers.All(m => context.HttpContext.Request.Headers.ContainsKey(m)))
            {
                context.Result = new JsonResult(new { Error = "Headers Missing" }) { StatusCode = 400 };
                return;

                throw new NotImplementedException();
            }
            ResourceExecutedContext executedContext = await next();
        }
    }
}
