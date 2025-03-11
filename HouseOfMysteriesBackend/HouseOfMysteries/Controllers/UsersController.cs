using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HouseOfMysteries.Models;
using HouseOfMysteries.Controllers;
using HouseOfMysteries.Classes;
using HouseOfMysteries.DTOs;
using Microsoft.EntityFrameworkCore;
using HouseOfMysteries;

namespace HouseOfMysteries.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> GetUsers(string token)
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
                return BadRequest("Nincs jogosultsága ehhez a művelethez!");    
            
            }





            
        }







    }
}
