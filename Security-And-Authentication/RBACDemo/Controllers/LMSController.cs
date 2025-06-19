using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RBACDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LMSController : ControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpPost("users")]
        public IActionResult ManageUsers()
        {
            return Ok("Admin: Managing users");
        }

        [Authorize(Roles = "Instructor")]
        [HttpPost("courses")]
        public IActionResult ManageCourses()
        {
            return Ok("Instructor: Managing own courses");
        }

        [Authorize(Roles = "Student")]
        [HttpGet("courses")]
        public IActionResult ViewCourses()
        {
            return Ok("Student: Viewing enrolled courses");
        }

        [Authorize(Roles = "Guest")]
        [HttpGet("public-courses")]
        public IActionResult ViewPublicCourses()
        {
            return Ok("Guest: Viewing public course information");
        }
    }
}