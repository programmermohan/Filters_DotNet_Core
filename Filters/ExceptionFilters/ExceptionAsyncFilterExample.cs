﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DotNetCore_Filters.Filters.ExceptionFilters
{
    public class ExceptionAsyncFilterExample : Attribute, IAsyncExceptionFilter
    {
        private readonly ILogger _logger;
        public ExceptionAsyncFilterExample(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("ExceptionAsyncFilterExample");
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------//
        public Task OnExceptionAsync(ExceptionContext context)
        {
            try
            {
                var username = context.HttpContext.User.Identity.Name;
                var trace = context.ActionDescriptor.RouteValues; //dictionary
                string Controller = trace["Controller"];
                string ActionName = trace["Action"];

                var HostIP = context.HttpContext.Request.Host.Value;
                string StrAppendError = string.Empty;

                //we can define multiple exceptions like below
                if (context.Exception is NotImplementedException)
                {
                    StrAppendError = "NotImplementedException: " + "\n" + "Error Message: " + context.Exception.Message + "\n" +
                        "Inner Exception: " + context.Exception.InnerException + "\n" +
                        "Stack Trace: " + context.Exception.StackTrace + "\n" + "Source: " + context.Exception.Source;

                    var response = new HttpResponseMessage(HttpStatusCode.NotImplemented)
                    {
                        Content = new StringContent("not implemented"),
                        ReasonPhrase = "not implemented.Please Contact your Administrator."
                    };
                    context.Result = new JsonResult(response);
                }
                else if (context.Exception is DivideByZeroException)
                {
                    StrAppendError = "DivideByZeroException: " + "\n" + "Error Message: " + context.Exception.Message + "\n" +
                        "Inner Exception: " + context.Exception.InnerException + "\n" +
                        "Stack Trace: " + context.Exception.StackTrace + "\n" + "Source: " + context.Exception.Source;

                    var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent("Bad Request"),
                        ReasonPhrase = "Bad Request.Please Contact your Administrator."
                    };
                    context.Result = new JsonResult(response);

                }
                else if (context.Exception is KeyNotFoundException)
                {
                    StrAppendError = "NotFound: " + "\n" + "Error Message: " + context.Exception.Message + "\n" +
                       "Inner Exception: " + context.Exception.InnerException + "\n" +
                       "Stack Trace: " + context.Exception.StackTrace + "\n" + "Source: " + context.Exception.Source;
                    context.HttpContext.Response.StatusCode = 400;
                    var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent("not found"),
                        ReasonPhrase = "not found.Please Contact your Administrator."
                    };
                    context.Result = new JsonResult(response);
                }
                else if (context.Exception is UnauthorizedAccessException)
                {
                    StrAppendError = "UnauthorizedAccessException: " + "\n" + "Error Message: " + context.Exception.Message + "\n" +
                       "Inner Exception: " + context.Exception.InnerException + "\n" +
                       "Stack Trace: " + context.Exception.StackTrace + "\n" + "Source: " + context.Exception.Source;
                    context.HttpContext.Response.StatusCode = 401;
                    //context.Result = new UnauthorizedResult();

                    var response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                    {
                        Content = new StringContent("you are not authrized to get the access"),
                        ReasonPhrase = "not authorized to view.Please Contact your Administrator."
                    };
                    context.Result = new JsonResult(response);
                }
                else
                {
                    var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent("An unhandled exception was thrown by service"),
                        ReasonPhrase = "Internal Server Error.Please Contact your Administrator."
                    };
                    context.Result = new JsonResult(response);
                    context.HttpContext.Response.StatusCode = 500;
                }

                _logger.LogError(StrAppendError);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
