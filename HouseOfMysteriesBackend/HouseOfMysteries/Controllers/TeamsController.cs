﻿using HouseOfMysteries.DTOs;
using HouseOfMysteries.Models;
using Microsoft.AspNetCore.Mvc;

namespace HouseOfMysteries.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        #region DbContext
        private readonly HouseofmysteriesContext _context;
        public TeamsController(HouseofmysteriesContext context)
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
            else if (roleId > 1)
            {
                try
                {
                    return Ok(_context.Teams.ToList());
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
                try
                {
                    if (_context.Teams.FirstOrDefault(f => f.TeamName == teamName) != null)
                    {
                        return BadRequest("This teamname is already in use!");
                    }
                    else
                    {
                        Team team = new Team();
                        team.TeamName = teamName;
                        _context.Teams.Add(team);
                        await _context.SaveChangesAsync();
                        return Ok("Team added successfully!");
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
        public async Task<ActionResult> Delete(string token, int teamId)
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
                        Team? team = new Team { TeamId = teamId };
                        team = _context.Teams.FirstOrDefault(f => f.TeamId == teamId);
                        if (team == null)
                        {
                            return BadRequest("Team not found!");
                        }
                        else
                        {
                            _context.Remove(team);
                            await _context.SaveChangesAsync();
                            return Ok("Team successfully deleted!");
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
        public async Task<ActionResult> Put(string token, Team team)
        {
            LoggedInUserDTO user = Program.loggedInUsers.CheckTokenValidity(token).LoggedInUser;
            if (user.RoleId == -1)
            {
                return BadRequest("Invalid token!");
            }
            else if (user.RoleId > 1)
            {
                    try
                    {
                        Team mTeam = new Team();
                        mTeam = _context.Teams.FirstOrDefault(f => f.TeamId == team.TeamId);
                        if (mTeam == null)
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
                            mTeam.TeamName = team.TeamName;
                            _context.Update(mTeam);
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
        #region TeamRegistration
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
                    try
                    {
                        if (_context.Teams.FirstOrDefault(f => f.TeamName == teamName) != null)
                        {
                            return BadRequest("This team name is already in use!");
                        }
                        Team team = new Team { TeamName = teamName };
                        _context.Teams.Add(team);
                        await _context.SaveChangesAsync();
                        int teamId = (int)_context.Teams.FirstOrDefault(f => f.TeamName == teamName).TeamId;
                        User? mUser = new User();
                        mUser = _context.Users.FirstOrDefault(f => f.UserId == user.UserId);
                        if (mUser == null)
                        {
                            return BadRequest("Only registered users can create a new team!");
                        }
                        mUser.TeamId = teamId;
                        _context.Users.Update(mUser);
                        await _context.SaveChangesAsync();
                        return Ok("Team registration successful!");
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
        #region AddUserToTeam
        [HttpPut("AddUserToTeam/{token}")]
        public async Task<ActionResult> AddUserToTeam(string token, string userName, string teamName)
        {
            LoggedInUserDTO user = Program.loggedInUsers.CheckTokenValidity(token).LoggedInUser;
            if (user.RoleId == -1)
            {
                return BadRequest("Invalid token!");
            }
            else if (user.RoleId > 1)
            {
                    try
                    {
                        if (user.RoleId == 2 && user.TeamId == null) //registered user
                        {
                            return BadRequest("You are not a member of any team, so you cannot recruit team members!");
                        }
                        if (_context.Users.FirstOrDefault(f => f.NickName == userName) == null)
                        {
                            return BadRequest("Username not found!");
                        }
                        User? mUser = new User();
                        mUser = _context.Users.FirstOrDefault(f => f.NickName == userName);

                        mUser.TeamId = user.TeamId;


                        if (user.RoleId > 2 && teamName != null) //collegue and admin
                        {
                            if (_context.Teams.FirstOrDefault(f => f.TeamName == teamName) == null)
                            {
                                return BadRequest("Team not found!");
                            }
                            mUser.TeamId = _context.Teams.FirstOrDefault(f => f.TeamName == teamName).TeamId;
                        }

                        string emailMessage = $"Kedves {mUser.NickName}!\n\nEzt a levelet azért kaptad, mert a Rejtélyekháza  {teamName} nevü csapata szeretne felkérni, hogy csatlakozz hozzájuk.\nAzt tudnod kell, hogy ha elfogadod a felkérést, akkor jelenlegi csapatodtól el kell búcsúznod, mert egyszerre csak egy csapat színeiben versenyezhetsz.\nHa szeretnél hozzájuk csatlakozni, csupán annyit kell tenned, hogy rákattintasz az alábbi linkre:\n https://localhost:3000/Users/AcceptInvitation?felhasznaloNev={mUser.NickName}&teamName={teamName}\nHa nem szeretnél csapatot váltani, akkor semmilyen teendőd nincsen.\n\nÜdvözlettel: Rejtélyek háza";


                        await Program.SendEmail(mUser.Email, "Meghívó", emailMessage);
                        return Ok("Invitation sent!");
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
        #region AcceptInvitation
        [HttpPut("AcceptInvitation/{userName},{teamName}")]
        public async Task<ActionResult> AcceptInvitation(string userName, string teamName)
        {
                try
                {
                    User? mUser = new User();
                    mUser = _context.Users.FirstOrDefault(f => f.NickName == userName);
                    mUser.TeamId = _context.Teams.FirstOrDefault(f => f.TeamName == teamName).TeamId;
                    _context.Users.Update(mUser);
                    await _context.SaveChangesAsync();
                    return Ok("User successfully added to team!");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
        }
        #endregion
    }
}
