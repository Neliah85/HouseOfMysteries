using Microsoft.AspNetCore.Mvc;

namespace HouseOfMysteries.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LogoutController : ControllerBase
    {
        [HttpPost("{userName}")]
        public IActionResult Logout(string userName)
        {
            try
            {
                Program.loggedInUsers.Logout(userName);
                return Ok(userName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
