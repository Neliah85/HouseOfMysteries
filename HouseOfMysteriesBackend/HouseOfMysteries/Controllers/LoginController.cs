using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HouseOfMysteries.Models;
using HouseOfMysteries.DTOs;
using System.Text.Json;




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

/*

dcedbd2d352d19c6eae0dfb12271b74d985c825b8d774afd2abd0d101b6e57ef

Id:c7a8721e-097d-408c-bbce-cae69b025ca7
UserName:LakatosI
Salt:jQGX8grO1yjNqhiZbtROcseiqj1NVZJd2iqlfxPx1GKLJ9H8smnLJ9dloScCK6Zp
Hash:dcedbd2d352d19c6eae0dfb12271b74d985c825b8d774afd2abd0d101b6e57ef
Email:lakatosi@gmail.com 
*/