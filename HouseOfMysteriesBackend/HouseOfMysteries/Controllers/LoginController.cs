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
    #region DbContext
    private readonly HouseofmysteriesContext _context;
    public LoginController(HouseofmysteriesContext context)
    {
        _context = context;
    }
    #endregion
    #region GetSalt
    [HttpPost("GetSalt/{userName}")]
    public async Task<IActionResult> GetSalt(string userName)
    {
        try
        {
            User? response = await _context.Users.FirstOrDefaultAsync(f => f.NickName == userName);
            return response == null ? BadRequest("Error") : Ok(response.Salt);
        }
        catch
        (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    #endregion
    #region Login
    [HttpPost]
    public async Task<IActionResult> Login(LoginDTO loginDTO)
    {
        try
        {
            string Hash = Program.CreateSHA256(loginDTO.TmpHash);
            Hash = Program.CreateSHA256(Hash);
            User? loginUser = await _context.Users.FirstOrDefaultAsync(f => f.NickName == loginDTO.LoginName && f.Hash == Hash);
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
    #endregion
}
