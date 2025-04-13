using HouseOfMysteries.DTOs;
using HouseOfMysteries.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace HouseOfMysteries.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        #region DbContext
        private readonly HouseofmysteriesContext _context;
        public BookingController(HouseofmysteriesContext context)
        {
            _context = context;
        }
        #endregion
        #region CheckBooking
        [HttpGet("CheckBooking{token}")]
        public IActionResult CheckBooking(string token, DateTime day, int roomId)
        {
            int? roleId = Program.loggedInUsers.CheckTokenValidity(token).LoggedInUser.RoleId;
            if (roleId == -1)
            {
                return BadRequest("Invalid token!");
            }
            else if (roleId > 1)
            {
                string date = DateOnly.FromDateTime(day).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "%";
                try
                {
                    return Ok(_context.Bookings.FromSql($"SELECT * FROM `booking` WHERE roomId = {roomId} AND isAvailable = 0 AND bookingDate LIKE {date}").ToList());
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
        #region GetAllBooking
        [HttpGet("GetAllBooking{token}")]
        public IActionResult GetAllBooking(string token)
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
                    return Ok(_context.Bookings.ToList());
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
        #region ClearBooking
        [HttpPut("ClearBooking/{token}")]
        public async Task<IActionResult> Put(string token, ClearBookingDTO clearBooking)
        {
            int? roleId = Program.loggedInUsers.CheckTokenValidity(token).LoggedInUser.RoleId;
            if (roleId == -1)
            {
                return BadRequest("Invalid token!");
            }
            else if (roleId > 1)
            {
                Booking? booking = new Booking();
                try
                {
                    booking = _context.Bookings.FirstOrDefaultAsync(f => f.BookingDate.Equals(clearBooking.BookingDate) && f.RoomId == clearBooking.RoomId && f.IsAvailable == false).Result;
                    if (booking != null)
                    {
                        booking.IsAvailable = true;
                        _context.Bookings.Update(booking);
                        await _context.SaveChangesAsync();
                        return Ok("Clear booking succesful!");
                    }
                    else
                    {
                        return Ok("Booking not found!");
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
        #region SaveResult
        [HttpPut("SaveResult/{token}")]
        public async Task<IActionResult> SaveResult(string token, SaveResultDTO newResult)
        {
            int? roleId = Program.loggedInUsers.CheckTokenValidity(token).LoggedInUser.RoleId;
            if (roleId == -1)
            {
                return BadRequest("Invalid token!");
            }
            else if (roleId > 2)
            {
                Booking? booking = new Booking();
                try
                {
                    booking = _context.Bookings.FirstOrDefaultAsync(f => f.BookingDate.Equals(newResult.BookingDate) && f.RoomId == newResult.RoomId).Result;
                    if (booking != null)
                    {
                        booking.Result = newResult.Result;
                        _context.Bookings.Update(booking);
                        await _context.SaveChangesAsync();
                        return Ok("Save result succesful!");
                    }
                    else
                    {
                        return Ok("Game not found!");
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
        #region NewBooking
        [HttpPost("NewBooking/{token}")]
        public async Task<IActionResult> NewBooking(string token, BookingDTO newBooking)
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
                        Booking? booking = new Booking();
                        booking = _context.Bookings.FirstOrDefaultAsync(f => f.BookingDate.Equals(newBooking.BookingDate) && f.RoomId == newBooking.RoomId).Result;
                        if (booking != null)
                        {
                            if (booking.IsAvailable == false)
                            {
                                return Ok("The room cannot be booked for this time!");
                            }
                            else
                            {
                                booking.TeamId = newBooking.TeamId;
                                booking.Comment = newBooking.Comment;
                                booking.IsAvailable = false;
                                _context.Bookings.Update(booking);
                                await _context.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            booking = new Booking();
                            booking.BookingDate = newBooking.BookingDate;
                            booking.RoomId = newBooking.RoomId;
                            booking.TeamId = newBooking.TeamId;
                            booking.Comment = newBooking.Comment;
                            booking.IsAvailable = false;
                            await _context.AddAsync(booking);
                            await _context.SaveChangesAsync();
                        }
                        return Ok("Booking successful!");
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
        [HttpDelete("{token}")]
        public async Task<IActionResult> Delete(string token, int bookingId)
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
                    Booking booking = new Booking { BookingId = bookingId };
                    _context.Remove(booking);
                    await _context.SaveChangesAsync();
                    return Ok("Delete succesful!");
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
        #region TeamCompetition
        [HttpGet("TeamCompetition/{roomId}")]
        public ActionResult TeamCompetition(int roomId, int? limit)
        {
            try
            {
                if (limit == 0 || limit == null)
                {
                    return Ok(_context.Bookings.FromSql($"SELECT * FROM `booking` WHERE(result IS NOT null)AND(roomId = {roomId})AND(teamId IS NOT null) ORDER by result").ToList());
                }
                else
                {
                    return Ok(_context.Bookings.FromSql($"SELECT * FROM `booking` WHERE(result IS NOT null)AND(roomId = {roomId})AND(teamId IS NOT null) ORDER by result LIMIT {limit}").ToList());
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}


