using Microsoft.AspNetCore.Mvc;
using HouseOfMysteries.Models;
using Microsoft.EntityFrameworkCore;

namespace HouseOfMysteries.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        #region DbContext
        private readonly HouseofmysteriesContext _context;
        public UsersController(HouseofmysteriesContext context)
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
            else if (roleId > 2)
            {
                try
                {
                    //List<User>users = new List<User>();
                    var response = _context.Users.Include(f => f.Team).Include(f => f.Role).Select(f => new
                    {
                        f.UserId,
                        f.RealName,
                        f.NickName,
                        f.Email,
                        f.Phone,
                        f.TeamId,
                        f.RoleId,
                        f.Salt,
                        f.Hash,
                        f.Team.TeamName,
                        f.Role.RoleName
                    }
                    ).ToList();
                    return Ok(response);
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
        #region GetByUserName
        [HttpGet("GetByUserName/{token},{userName}")]
        public ActionResult GetByUserName(string token, string userName)
        {
            int? roleId = Program.loggedInUsers.CheckTokenValidity(token).LoggedInUser.RoleId;
            if (roleId == -1)
            {
                return BadRequest("Invalid token!");
            }
            else if (roleId > 2)
            {
                try
                {
                    var response = _context.Users.Include(f => f.Team).Include(f => f.Role).Select(f => new
                    {
                        f.UserId,
                        f.RealName,
                        f.NickName,
                        f.Email,
                        f.Phone,
                        f.TeamId,
                        f.RoleId,
                        f.Salt,
                        f.Hash,
                        f.Team.TeamName,
                        f.Role.RoleName
                    }).FirstOrDefault(f => f.NickName == userName);
                    if (response != null)
                    {
                        return Ok(response);
                    }
                    else
                    {
                        return BadRequest("User not found!");
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
        #region Post
        [HttpPost("{token}")]
        public async Task<IActionResult> Post(string token, User user)
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
                    user.UserId = 0;
                    user.TeamId = null;
                    user.RoleId = null;
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    return Ok("User added successfully!");
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
        #region Delete
        [HttpDelete("{token},{userId}")]
        public async Task<ActionResult> Delete(string token, int userId)
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
                    User user = new User { UserId = userId };
                    _context.Remove(user);
                    await _context.SaveChangesAsync();
                    return Ok("User successfully deleted!");
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
        #region DeleteByUserName
        [HttpDelete("DeleteByUserName/{token},{userName}")]
        public async Task<ActionResult> DeleteByUserName(string token, string userName)
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
                    User? user = new User();
                    user = _context.Users.FirstOrDefault(f => f.NickName == userName);
                    if (user == null)
                    {
                        return BadRequest("Username not found!");
                    }
                    else
                    {
                        _context.Remove(user);
                        await _context.SaveChangesAsync();
                        return Ok("User successfully deleted!");
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
        [HttpPut("{token},{user}")]
        public async Task<ActionResult> Put(string token, User user)
        {
            User? mUser = new User();
            int? roleId = Program.loggedInUsers.CheckTokenValidity(token).LoggedInUser.RoleId;
            if (roleId == -1)
            {
                return BadRequest("Invalid token!");
            }
            else if (roleId > 1)
            {
                try
                {
                    mUser = _context.Users.FirstOrDefault(f => f.UserId == user.UserId);
                    if (mUser == null)
                    {
                        return BadRequest("User not found!");
                    }
                    else
                    {
                        mUser.RealName = user.RealName;
                        mUser.NickName = user.NickName;
                        mUser.Email = user.Email;
                        mUser.Phone = user.Phone;
                        if (mUser.TeamId != user.TeamId && roleId < 3)
                        {
                            return BadRequest("You do not have permission to modify TeamId!");
                        }
                        else
                        {
                            mUser.TeamId = user.TeamId;
                        }
                        if (mUser.RoleId != user.RoleId && roleId < 4)
                        {
                            return BadRequest("You do not have permission to modify TeamId!");
                        }
                        else
                        {
                            mUser.RoleId = user.RoleId;
                        }
                        _context.Users.Update(mUser);
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
        #region UpdateUser
        [HttpPut("UpdateUser/{token}")]
        public async Task<ActionResult> UpdateUser(string token, User user)
        {
            User? mUser = new User();
            int? roleId = Program.loggedInUsers.CheckTokenValidity(token).LoggedInUser.RoleId;
            if (roleId == -1)
            {
                return BadRequest("Invalid token!");
            }
            else if (roleId > 1)
            {
                try
                {
                    mUser = _context.Users.FirstOrDefault(f => f.UserId == user.UserId);
                    if (mUser == null)
                    {
                        return BadRequest("User not found!");
                    }
                    else
                    {
                        mUser.RealName = user.RealName;
                        mUser.NickName = user.NickName;
                        mUser.Email = user.Email;
                        mUser.Phone = user.Phone;
                        if (mUser.TeamId != user.TeamId && roleId < 3)
                        {
                            return BadRequest("You do not have permission to modify TeamId!");
                        }
                        else
                        {
                            mUser.TeamId = user.TeamId;
                        }
                        if (mUser.RoleId != user.RoleId && roleId < 4)
                        {
                            return BadRequest("You do not have permission to modify TeamId!");
                        }
                        else
                        {
                            mUser.RoleId = user.RoleId;
                        }
                        _context.Users.Update(mUser);
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

