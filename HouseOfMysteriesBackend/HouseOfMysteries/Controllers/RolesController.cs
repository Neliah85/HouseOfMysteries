using HouseOfMysteries.Models;
using Microsoft.AspNetCore.Mvc;

namespace HouseOfMysteries.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        #region DbContext
        private readonly HouseofmysteriesContext _context;
        public RolesController(HouseofmysteriesContext context)
        {
            _context = context;
        }
        #endregion
        #region Get
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
                try
                {
                    return Ok(_context.Roles.ToList());
                }
                catch (Exception ex)
                {
                    return BadRequest($"{ex.Message}");
                }
            }
            else
            {
                return BadRequest("You do not have permission to perform this operation!");
            }
        }
        #endregion
        #region Post
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
                try
                {
                    if (_context.Roles.FirstOrDefault(f => f.RoleName == roleName) != null)
                    {
                        return BadRequest("This role name is already in use!");
                    }
                    else
                    {
                        Role role = new Role();
                        role.RoleName = roleName;
                        _context.Roles.Add(role);
                        await _context.SaveChangesAsync();
                        return Ok("Role added successfully!");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest($"{ex.Message}");
                }
            }
            else
            {
                return BadRequest("You do not have permission to perform this operation!");
            }
        }
        #endregion
        #region Delete
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
                try
                {
                    Role role = new Role { RoleId = id };
                    role = _context.Roles.FirstOrDefault(f => f.RoleId == id);
                    if (role == null)
                    {
                        return BadRequest("RoleId not found!");
                    }
                    else
                    {

                        _context.Remove(role);
                        await _context.SaveChangesAsync();
                        return Ok("Role successfully deleted!");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest("You do not have permission to perform this operation!");
            }
        }
        #endregion
        #region Put
        [HttpPut("{token}")]
        public async Task<ActionResult> Put(string token, Role role)
        {
            Role? mRole = new Role(); 
            int? roleId = Program.loggedInUsers.CheckTokenValidity(token).LoggedInUser.RoleId;
            if (roleId == -1)
            {
                return BadRequest("Invalid token!");
            }
            else if (roleId > 3)
            {
                try
                {
                    mRole = _context.Roles.FirstOrDefault(f => f.RoleId == role.RoleId);
                    if (mRole == null)
                    {
                        return BadRequest("RoleId not found!");
                    }
                    else
                    {
                        mRole.RoleName = role.RoleName;
                        _context.Update(mRole);
                        await _context.SaveChangesAsync();
                        return Ok("The modification was successful!");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest("You do not have permission to perform this operation!");
            }
        }
        #endregion
    }
}
