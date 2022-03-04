using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore_Filters.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        [HttpGet]
        [Route("GetAllStudents")]
        public IActionResult GetStudents()
        {
            return Ok(new List<string>()
            {
                "Student01", "Student02", "Student03", "Student04", "Student05"
            });
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------//

        [HttpGet]
        [Route("GetStudentById")]
        public IActionResult GetStudentById()
        {
            return Ok("Student02");
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------//
    }
}
