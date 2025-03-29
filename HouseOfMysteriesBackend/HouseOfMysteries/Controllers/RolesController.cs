using HouseOfMysteries.Models;
using Microsoft.AspNetCore.Mvc;

namespace HouseOfMysteries.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        [HttpGet("{token}")]
        public ActionResult Get(string token)
        {
            int? roleId = Program.loggedInUsers.CheckTokenValidity(token).LoggedInUser.RoleId;
            if (roleId == -1)
            {
                return BadRequest("Invalid token!");
            }
            else if (roleId > 3)
            {
                using (var context = new HouseofmysteriesContext())
                {
                    try
                    {
                        return Ok(context.Roles.ToList());
                    }
                    catch (Exception ex)
                    {
                        return BadRequest($"{ex.Message}");
                    }
                }
            }
            else
            {
                return BadRequest("You do not have permission to perform this operation!");
            }
        }

        [HttpPost("{token},{roleName}")]
        public async Task<ActionResult> Post(string token, string roleName)
        {
            int? roleId = Program.loggedInUsers.CheckTokenValidity(token).LoggedInUser.RoleId;
            if (roleId == -1)
            {
                return BadRequest("Invalid token!");
            }
            else if (roleId > 3)
            {
                using (var context = new HouseofmysteriesContext())
                {
                    try
                    {
                        if (context.Roles.FirstOrDefault(f => f.RoleName == roleName) != null)
                        {
                            return BadRequest("This role name is already in use!");
                        }
                        else
                        {
                            Role role = new Role();
                            role.RoleName = roleName;
                            context.Roles.Add(role);
                            await context.SaveChangesAsync();
                            return Ok("Role added successfully!");
                        }
                    }
                    catch (Exception ex)
                    {
                        return BadRequest($"{ex.Message}");
                    }
                }
            }
            else
            {
                return BadRequest("You do not have permission to perform this operation!");
            }
        }

        [HttpDelete("{token}")]
        public async Task<ActionResult> Delete(string token, int id)
        {
            int? roleId = Program.loggedInUsers.CheckTokenValidity(token).LoggedInUser.RoleId;
            if (roleId == -1)
            {
                return BadRequest("Invalid token!");
            }
            else if (roleId > 3)
            {
                using (var context = new HouseofmysteriesContext())
                {
                    try
                    {
                        if (context.Roles.FirstOrDefault(f => f.RoleId == id) == null)
                        {
                            return BadRequest("RoleId not found!");
                        }
                        else
                        {
                            Role role = new Role { RoleId = id };
                            context.Remove(role);
                            await context.SaveChangesAsync();
                            return Ok("Role successfully deleted!");
                        }
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

        [HttpPut("{token},{role}")]
        public async Task<ActionResult> Put(string token, Role role)
        {
            int? roleId = Program.loggedInUsers.CheckTokenValidity(token).LoggedInUser.RoleId;
            if (roleId == -1)
            {
                return BadRequest("Invalid token!");
            }
            else if (roleId > 3)
            {
                using (var context = new HouseofmysteriesContext())
                {
                    try
                    {
                        if (context.Roles.FirstOrDefault(f => f.RoleId == role.RoleId) == null)
                        {
                            return BadRequest("RoleId not found!");
                        }
                        else
                        {
                            Role mRole = new Role { RoleId = role.RoleId};
                            context.Update(role);
                            await context.SaveChangesAsync();
                            return Ok("The modification was successful!");
                        }
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
