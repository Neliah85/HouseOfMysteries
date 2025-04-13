using HouseOfMysteries.Models;
using Microsoft.AspNetCore.Mvc;

namespace HouseOfMysteries.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        #region DbContext
        private readonly HouseofmysteriesContext _context;
        public RoomsController(HouseofmysteriesContext context)
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
                    return Ok(_context.Rooms.ToList());
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
                try
                {
                    if (_context.Rooms.FirstOrDefault(f => f.RoomName == roomName) != null)
                    {
                        return BadRequest("This room name is already in use!");
                    }
                    else
                    {
                        Room room = new Room();
                        room.RoomName = roomName;
                        _context.Rooms.Add(room);
                        await _context.SaveChangesAsync();
                        return Ok("Room added successfully!");
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
                        Room? room = new Room { RoomId = id };
                        room = _context.Rooms.FirstOrDefault(f => f.RoomId == id);
                        if (room == null)
                        {
                            return BadRequest("RoomId not found!");
                        }
                        else
                        {
                            _context.Remove(room);
                            await _context.SaveChangesAsync();
                            return Ok("Room successfully deleted!");
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
        public async Task<ActionResult> Put(string token, Room room)
        {
            int? roleId = Program.loggedInUsers.CheckTokenValidity(token).LoggedInUser.RoleId;
            if (roleId == -1)
            {
                return BadRequest("Invalid token!");
            }
            else if (roleId > 3)
            {
                Room? mRoom = new Room();
                try
                    {
                    mRoom = _context.Rooms.FirstOrDefault(f => f.RoomId == room.RoomId);
                        if (mRoom == null)
                        {
                            return BadRequest("RoomId not found!");
                        }
                        else
                        {
                        mRoom.RoomName = room.RoomName;
                            _context.Update(mRoom);
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
