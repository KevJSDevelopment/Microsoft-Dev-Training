using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RBACDemo.Data;

namespace RBACDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SeedController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("users")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SeedUsers()
        {
            // LMS Roles and Users
            var lmsRoles = new[] { "Admin", "Instructor", "Student", "Guest" };
            var lmsUsers = new[]
            {
                new { Username = "lmsadmin", Password = "Admin@123", Role = "Admin" },
                new { Username = "instructor1", Password = "Instr@123", Role = "Instructor" },
                new { Username = "student1", Password = "Stud@123", Role = "Student" },
                new { Username = "guest1", Password = "Guest@123", Role = "Guest" }
            };

            // Retail Bank Roles and Users
            var bankRoles = new[] { "Admin", "Teller", "Auditor", "Customer" };
            var bankUsers = new[]
            {
                new { Username = "bankadmin", Password = "Admin@123", Role = "Admin" },
                new { Username = "teller1", Password = "Tell@123", Role = "Teller" },
                new { Username = "auditor1", Password = "Audi@123", Role = "Auditor" },
                new { Username = "customer1", Password = "Cust@123", Role = "Customer" }
            };

            // Ensure roles exist
            foreach (var role in lmsRoles.Concat(bankRoles))
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Create and assign users
            foreach (var user in lmsUsers.Concat(bankUsers))
            {
                var identityUser = new IdentityUser { UserName = user.Username, Email = $"{user.Username}@example.com" };
                var result = await _userManager.CreateAsync(identityUser, user.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(identityUser, user.Role);
                }
            }

            return Ok("Users and roles seeded successfully.");
        }
    }
}