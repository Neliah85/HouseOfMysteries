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
                        if (mUser == null) 
                        {
                            return BadRequest("Only registered users can create a new team!");
                        }
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
                        if (user.RoleId == 2 && user.TeamId == null) //registered user
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


                        if (user.RoleId > 2 && teamName != null) //collegue and admin
                        {
                            if (context.Teams.FirstOrDefault(f => f.TeamName == teamName) == null)
                            {
                                return BadRequest("Team not found!");
                            }
                            mUser.TeamId = context.Teams.FirstOrDefault(f => f.TeamName == teamName).TeamId;
                        }

                        Program.SendEmail(mUser.Email, "Meghívó", $"Kedves {mUser.NickName}!\n\nEzt a levelet azért kaptad, mert a Rejtélyekháza  {teamName} nevü csapata szeretne felkérni, hogy csatlakozz hozzájuk.\nAzt tudnod kell, hogy ha elfogadod a felkérést, akkor jelenlegi csapatodtól el kell búcsúznod, mert egyszerre csak egy csapat színeiben versenyezhetsz.\nHa szeretnél hozzájuk csatlakozni, csupán annyit kell tenned, hogy rákattintasz az alábbi linkre:\n https://localhost:5131/Users/AcceptInvitation?felhasznaloNev={mUser.NickName}&teamName={teamName}\nHa nem szeretnél csapatot váltani, akkor semmilyen teendőd nincsen.\n\nÜdvözlettel: Rejtélyek háza");
                        return Ok("Invitation sent!");
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

        [HttpPut("AcceptInvitation/{userName},{teamName}")]
        public async Task<ActionResult> AcceptInvitation(string userName, string teamName)
        {
            using (var context = new HouseofmysteriesContext())
            {
                try
                {
                    User? mUser = new User();
                    mUser = context.Users.FirstOrDefault(f => f.NickName == userName);
                    mUser.TeamId = context.Teams.FirstOrDefault(f => f.TeamName == teamName).TeamId;
                    context.Users.Update(mUser);
                    await context.SaveChangesAsync();
                    return Ok("User successfully added to team!");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
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