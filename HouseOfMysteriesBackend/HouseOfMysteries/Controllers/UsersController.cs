using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HouseOfMysteries.Models;


namespace HouseOfMysteries.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet("{token}")]
        public ActionResult Get(string token)
        {
            if (Program.loggedInUsers.CheckTokenValidity(token).LoggedInUser.RoleId > 2)
            {
                using (var context = new HouseofmysteriesContext())
                {
                    try
                    {
                        return Ok(context.Users.ToList());
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                }
            }
            else
            {
                return BadRequest("You do not have permission to perform this operation!");
            }
        }

        [HttpGet("{token},{userName}")]
        public ActionResult GetUserName(string token, string userName)
        {
            if (Program.loggedInUsers.CheckTokenValidity(token).LoggedInUser.RoleId > 2)
            {
                using (var context = new HouseofmysteriesContext())
                {
                    try
                    {
                        return Ok(context.Users.FirstOrDefault(f => f.NickName == userName));
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                }
            }
            else
            {
                return BadRequest("You do not have permission to perform this operation!");
            }
        }

        [HttpPost("{token}")]
        public async Task<IActionResult> Post(string token, User user)
        {
            if (Program.loggedInUsers.CheckTokenValidity(token).LoggedInUser.RoleId > 3)
            {
                using (var context = new HouseofmysteriesContext())
                {
                    try
                    {
                        user.UserId = 0;
                        user.TeamId = null;
                        user.RoleId = null;    
                        context.Add(user);
                        await context.SaveChangesAsync();
                        return Ok("User added successfully!");
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                }
            }
            else
            {
                return BadRequest("You do not have permission to perform this operation!");
            }
        }
    }
}
