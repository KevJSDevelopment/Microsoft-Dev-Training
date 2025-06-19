using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RBACDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankController : ControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpPost("accounts")]
        public IActionResult ManageAccounts()
        {
            return Ok("Admin: Managing accounts");
        }

        [Authorize(Roles = "Teller")]
        [HttpPost("transactions")]
        public IActionResult ProcessTransactions()
        {
            return Ok("Teller: Processing transactions");
        }

        [Authorize(Roles = "Auditor")]
        [HttpGet("logs")]
        public IActionResult ViewLogs()
        {
            return Ok("Auditor: Viewing system logs");
        }

        [Authorize(Roles = "Customer")]
        [HttpGet("account-details")]
        public IActionResult ViewAccountDetails()
        {
            return Ok("Customer: Viewing personal account details");
        }
    }
}