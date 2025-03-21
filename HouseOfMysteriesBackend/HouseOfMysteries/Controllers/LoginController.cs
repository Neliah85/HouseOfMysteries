using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HouseOfMysteries.Models;
using HouseOfMysteries.DTOs;

namespace HouseOfMysteries.Controllers;

[Route("[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    [HttpPost("GetSalt/{userName}")]
    public async Task<IActionResult> GetSalt(string userName)
    {
        using (var context = new HouseofmysteriesContext())
        {
            try
            {
                User? response = await context.Users.FirstOrDefaultAsync(f => f.NickName == userName);
                return response == null ? BadRequest("Error") : Ok(response.Salt);
            }
            catch
            (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
    [HttpPost]
    public async Task<IActionResult> Login(LoginDTO loginDTO)
    {
        using (var context = new HouseofmysteriesContext())
        {
            try
            {
                string Hash = Program.CreateSHA256(loginDTO.TmpHash);
                Hash = Program.CreateSHA256(Hash);
                User? loginUser = await context.Users.FirstOrDefaultAsync(f => f.NickName == loginDTO.LoginName && f.Hash == Hash);
                if (loginUser != null)
                {
                    return Ok(Program.loggedInUsers.GenerateToken(3600, loginUser));
                }
                else
                {
                    return BadRequest("Incorrect username or password!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}
