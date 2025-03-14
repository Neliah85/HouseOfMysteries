using Microsoft.AspNetCore.Mvc;

namespace HouseOfMysteries.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LogoutController : ControllerBase
    {
        [HttpPost("{token}")]
        public IActionResult Logout(string token)
        {
            try
            {
                Program.loggedInUsers.Logout(token);
                return Ok("Logout succesful!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
