using HouseOfMysteries.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HouseOfMysteries.DTOs;
using static System.Net.WebRequestMethods;

namespace HouseOfMysteries.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RegistryController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult>Registry(User user) 
        {
            using (var context = new HouseofmysteriesContext())
            {
                try
                {
                    if (context.Users.FirstOrDefault(f => f.NickName == user.NickName) != null)
                    {
                        return Ok("This username is already in use!");
                    }
                    if (context.Users.FirstOrDefault(f => f.Email == user.Email) != null)
                    {
                        return Ok("This e-mail address is already in use!");
                    }
                    user.RoleId = 1;
                    user.Hash = Program.CreateSHA256(user.Hash);
                    await context.Users.AddAsync(user);
                    await context.SaveChangesAsync();

                    string emailBody = $"Kedves {user.NickName}!\n\nEzt a levelet azért kaptad, mert regisztráltál a weboldalunkon. Regisztrációdat az alábbi linkre kattintva erősítheted meg:\n https://localhost:3000/Registry?felhasznaloNev={{user.NickName}}&email={{user.Email}}\nHa nem Te kezdeményezted a regisztrációt, levelünket hagyd figyelmen kívül!\nÜdvözlettel: Rejtélyek háza";

                    _ = Program.SendEmail(user.Email, "Regisztráció", emailBody);
                    return Ok("Successful registration. Complete your registration using the link sent to your email address!");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        
        [HttpPost("Confirm")]
        public async Task<IActionResult> EndOfTheRegistry(ConfirmRegDTO confirmReg )
        {
            using (var context = new HouseofmysteriesContext())
            {
                try
                {
                    User? user = await context.Users.FirstOrDefaultAsync(f => f.NickName == confirmReg.LoginName && f.Email == confirmReg.Email);
                    if (user == null)
                    {
                        return Ok("Failed to verify registration!");
                    }
                    else
                    {
                        if (user.RoleId == 1)
                        {
                            user.RoleId = 2;
                            context.Users.Update(user);
                            await context.SaveChangesAsync();
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
        }

    }
}
