using HouseOfMysteries.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HouseOfMysteries.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
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
                        return Ok(context.Rooms.ToList());
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

        [HttpPost("{token},{roomName}")]
        public async Task<ActionResult> Post(string token, string roomName)
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
                        if (context.Rooms.FirstOrDefault(f => f.RoomName == roomName) != null)
                        {
                            return BadRequest("This room name is already in use!");
                        }
                        else
                        {
                            Room room = new Room();
                            room.RoomName = roomName;
                            context.Rooms.Add(room);
                            await context.SaveChangesAsync();
                            return Ok("Room added successfully!");
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
                        if (context.Rooms.FirstOrDefault(f => f.RoomId == id) == null)
                        {
                            return BadRequest("RoomId not found!");
                        }
                        else
                        {
                            Room room = new Room { RoomId = id };
                            context.Remove(room);
                            await context.SaveChangesAsync();
                            return Ok("Room successfully deleted!");
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


        [HttpPut("{token},{room}")]
        public async Task<ActionResult> Put(string token, Room room)
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
                        if (context.Rooms.FirstOrDefault(f => f.RoomId == room.RoomId) == null)
                        {
                            return BadRequest("RoomId not found!");
                        }
                        else
                        {
                            Room mRoom = new Room { RoomId = room.RoomId };
                            context.Update(room);
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
