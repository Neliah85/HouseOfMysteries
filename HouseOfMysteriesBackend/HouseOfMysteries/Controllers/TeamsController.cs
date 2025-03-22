using HouseOfMysteries.DTOs;
using HouseOfMysteries.Models;
using Microsoft.AspNetCore.Mvc;

namespace HouseOfMysteries.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        [HttpGet("{token}")]
        public async Task<ActionResult> Get(string token)
        {
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
                        return Ok(context.Teams.ToList());
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

        [HttpPost("{token},{teamName}")]
        public async Task<ActionResult> Post(string token, string teamName)
        {
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
                        if (context.Teams.FirstOrDefault(f => f.TeamName == teamName) != null)
                        {
                            return BadRequest("This teamname is already in use!");
                        }
                        else
                        {
                            Team team = new Team();
                            team.TeamName = teamName; 
                            context.Teams.Add(team);
                            await context.SaveChangesAsync();
                            return Ok("Team added successfully!");
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
        public async Task<ActionResult> Delete(string token, int teamId)
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
                        if (context.Teams.FirstOrDefault(f => f.TeamId == teamId) == null)
                        {
                            return BadRequest("Team not found!");
                        }
                        else
                        {
                            Team team = new Team { TeamId = teamId };
                            context.Remove(team);
                            await context.SaveChangesAsync();
                            return Ok("Team successfully deleted!");
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

        [HttpPut("{token},{team}")]
        public async Task<ActionResult> Put(string token, Team team)
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
                        if (context.Teams.FirstOrDefault(f => f.TeamId == team.TeamId) == null)
                        {
                            return BadRequest("Team not found!");
                        }
                        else
                        {
                            if (user.RoleId == 2) 
                            {
                                if (team.TeamId != user.TeamId) 
                                {
                                    return BadRequest("You can only change the name of your own team!");
                                }
                            }
                            context.Update(team);
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

//int? roleId = Program.loggedInUsers.CheckTokenValidity(token).LoggedInUser.RoleId;
//if (roleId == -1)
//{
//    return BadRequest("Invalid token!");
//}
//else if (roleId > 1)
//{

//}
//else
//{
//    return BadRequest("You do not have permission to perform this operation!");
//}