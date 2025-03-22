using Microsoft.AspNetCore.Mvc;
using HouseOfMysteries.Models;
using HouseOfMysteries.DTOs;


namespace HouseOfMysteries.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
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
                using (var context = new HouseofmysteriesContext())
                {
                    try
                    {
                        User user = new User { UserId = userId };
                        context.Remove(user);
                        await context.SaveChangesAsync();
                        return Ok("User successfully deleted!");
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
                using (var context = new HouseofmysteriesContext())
                {
                    try
                    {
                        User? user = new User();
                        user = context.Users.FirstOrDefault(f => f.NickName == userName);
                        if (user == null)
                        {
                            return BadRequest("Username not found!");
                        }
                        else
                        {
                            context.Remove(user);
                            await context.SaveChangesAsync();
                            return Ok("User successfully deleted!");
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
                using (var context = new HouseofmysteriesContext())
                {
                    try
                    {
                        mUser = context.Users.FirstOrDefault(f => f.UserId == user.UserId);
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
                            context.Users.Update(mUser);
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

        [HttpPost("TeamRegistration/{token},{teamName}")]
        public async Task<ActionResult> TeamRegistration(string token, string teamName)
        {
            LoggedInUserDTO user = Program.loggedInUsers.CheckTokenValidity(token).LoggedInUser;
            if (user.RoleId == -1)
            {
                return BadRequest("Invalid token!");
            }
            else if (user.RoleId > 1)
            {
                using (var context = new HouseofmysteriesContext())
                {
                    try
                    {
                        if (context.Teams.FirstOrDefault(f => f.TeamName == teamName) != null)
                        {
                            return BadRequest("This team name is already in use!");
                        }
                        Team team = new Team { TeamName = teamName };
                        context.Teams.Add(team);
                        await context.SaveChangesAsync();
                        int teamId = context.Teams.FirstOrDefault(f => f.TeamName == teamName).TeamId;
                        User? mUser = new User();
                        mUser = context.Users.FirstOrDefault(f => f.UserId == user.UserId);
                        mUser.TeamId = teamId;
                        context.Users.Update(mUser);
                        await context.SaveChangesAsync();
                        return Ok("Team registration successful!");
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

        [HttpPut("AddUserToTeam/{token},{userName},teamName")]
        public async Task<ActionResult> AddUserToTeam(string token, string userName, string teamName)
        {
            LoggedInUserDTO user = Program.loggedInUsers.CheckTokenValidity(token).LoggedInUser;
            if (user.RoleId == -1)
            {
                return BadRequest("Invalid token!");
            }
            else if (user.RoleId > 1)
            {
                using (var context = new HouseofmysteriesContext())
                {
                    try
                    {
                        if (user.RoleId == 2 && user.TeamId == null)
                        {
                            return BadRequest("You are not a member of any team, so you cannot recruit team members!");
                        }
                        if (context.Users.FirstOrDefault(f => f.NickName == userName) == null)
                        {
                            return BadRequest("Username not found!");
                        }
                        User? mUser = new User();
                        mUser = context.Users.FirstOrDefault(f => f.NickName == userName);
                        mUser.TeamId = user.TeamId;
                        if (user.RoleId > 2 && teamName != null)
                        {
                            if (context.Teams.FirstOrDefault(f => f.TeamName == teamName) == null)
                            {
                                return BadRequest("Team not found!");
                            }
                            mUser.TeamId = context.Teams.FirstOrDefault(f => f.TeamName == teamName).TeamId;
                        }
                        context.Users.Update(mUser);
                        await context.SaveChangesAsync();
                        return Ok("User successfully added to team");
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
    }
}

