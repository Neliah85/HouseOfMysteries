using HouseOfMysteries.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HouseOfMysteries.DTOs;

namespace HouseOfMysteries.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RegistryController : ControllerBase
    {
        #region DbContext
        private readonly HouseofmysteriesContext _context;
        public RegistryController(HouseofmysteriesContext context)
        {
            _context = context;
        }
        #endregion
        #region Registry
        [HttpPost]
        public async Task<IActionResult>Registry(User user) 
        {
                try
                {
                    if (_context.Users.FirstOrDefault(f => f.NickName == user.NickName) != null)
                    {
                        return Ok("This username is already in use!");
                    }
                    if (_context.Users.FirstOrDefault(f => f.Email == user.Email) != null)
                    {
                        return Ok("This e-mail address is already in use!");
                    }
                    user.RoleId = 1;
                    user.Hash = Program.CreateSHA256(user.Hash);
                    await _context.Users.AddAsync(user);
                    await _context.SaveChangesAsync();

                    string emailBody = $"Kedves {user.NickName}!\n\nEzt a levelet azért kaptad, mert regisztráltál a weboldalunkon. Regisztrációdat az alábbi linkre kattintva erősítheted meg:\n https://localhost:3000/Registry?felhasznaloNev={{user.NickName}}&email={{user.Email}}\nHa nem Te kezdeményezted a regisztrációt, levelünket hagyd figyelmen kívül!\nÜdvözlettel: Rejtélyek háza";

                    _ = Program.SendEmail(user.Email, "Regisztráció", emailBody);
                    return Ok("Successful registration. Complete your registration using the link sent to your email address!");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
        }
        #endregion
        #region EndOfTheRegistry
        [HttpPost("Confirm")]
        public async Task<IActionResult> EndOfTheRegistry(ConfirmRegDTO confirmReg )
        {
                try
                {
                    User? user = await _context.Users.FirstOrDefaultAsync(f => f.NickName == confirmReg.LoginName && f.Email == confirmReg.Email);
                    if (user == null)
                    {
                        return Ok("Failed to verify registration!");
                    }
                    else
                    {
                        if (user.RoleId == 1)
                        {
                            user.RoleId = 2;
                            _context.Users.Update(user);
                            await _context.SaveChangesAsync();
                            return Ok("Registration completed successfully!");
                        }
                        else 
                        {
                            return BadRequest("The user has already confirmed their registration.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
        }
        #endregion
    }
}
