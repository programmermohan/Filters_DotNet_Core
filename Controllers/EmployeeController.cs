using DotNetCore_Filters.Auth;
using DotNetCore_Filters.Filters.ActionFilters;
using DotNetCore_Filters.Filters.ResultFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore_Filters.Controllers
{
    [Authorize(Roles = AuthorizedRole.Manager)]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        [HttpGet]
        [ServiceFilter(typeof(ActionFilterWithLogger))] //added Action filter
        [Route("GetEmployees")]        
        public IActionResult GetEmployees()
        {
            return Ok(new List<string>()
            {
                "Employee01", "Employee02", "Employee03", "Employee04", "Employee05"
            });
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------//
        
        [HttpGet]
        [Route("GetEmployeeById")]
        [ServiceFilter(typeof(ResultFilterExample))] //add Result Filter
        public IActionResult GetEmployeeById(long EmpId)
        {
            var employee = "Employee01";
            return Ok(employee);
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------//
    }
}
