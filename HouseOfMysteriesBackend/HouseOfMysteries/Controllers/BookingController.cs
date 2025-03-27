using HouseOfMysteries.DTOs;
using HouseOfMysteries.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mysqlx.Crud;
using MySqlX.XDevAPI.Common;
using System.Globalization;

namespace HouseOfMysteries.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
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
                using (var context = new HouseofmysteriesContext())
                {
                    try
                    {
                        return Ok(context.Bookings.FromSql($"SELECT * FROM `booking` WHERE roomId = {roomId} AND isAvailable = 0 AND bookingDate LIKE {date}").ToList());
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
                using (var context = new HouseofmysteriesContext())
                {
                    try
                    {
                        return Ok(context.Bookings.ToList());
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
                using (var context = new HouseofmysteriesContext())
                    try
                    {
                        booking = context.Bookings.FirstOrDefaultAsync(f => f.BookingDate.Equals(clearBooking.BookingDate) && f.RoomId == clearBooking.RoomId && f.IsAvailable == false).Result;
                        if (booking != null)
                        {
                            booking.IsAvailable = true;
                            context.Bookings.Update(booking);
                            await context.SaveChangesAsync();
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
                using (var context = new HouseofmysteriesContext())
                {
                    try
                    {
                        if (context.Bookings.FirstOrDefaultAsync(f => f.BookingDate.Equals(newResult.BookingDate) && f.RoomId == newResult.RoomId).Result != null)
                        {
                            booking = context.Bookings.FirstOrDefaultAsync(f => f.BookingDate.Equals(newResult.BookingDate) && f.RoomId == newResult.RoomId).Result;
                            booking.Result = newResult.Result;
                            context.Bookings.Update(booking);
                            await context.SaveChangesAsync();
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
            }
            else
            {
                return BadRequest("You do not have permission to perform this operation!");
            }
        }


        [HttpPost("NewBooking/{token}")]
        public async Task<IActionResult> NewBooking(string token,BookingDTO newBooking)
        {
            int? roleId = Program.loggedInUsers.CheckTokenValidity(token).LoggedInUser.RoleId;
            if (roleId == -1)
            {
                return BadRequest("Invalid token!");
            }
            else if (roleId > 1)
            {
                Booking? booking = new Booking();
                using (var context = new HouseofmysteriesContext())
                {
                    try
                    {
                        booking = context.Bookings.FirstOrDefaultAsync(f => f.BookingDate.Equals(newBooking.BookingDate) && f.RoomId == newBooking.RoomId && f.IsAvailable == false).Result;
                        if (booking != null)
                        {
                            return Ok("The room cannot be booked for this time!");
                        }

                        if (context.Bookings.FirstOrDefaultAsync(f => f.BookingDate.Equals(newBooking.BookingDate) && f.RoomId == newBooking.RoomId && f.IsAvailable == true).Result != null)
                        {
                            booking = context.Bookings.FirstOrDefaultAsync(f => f.BookingDate.Equals(newBooking.BookingDate) && f.RoomId == newBooking.RoomId && f.IsAvailable == true).Result;
                            booking.TeamId = newBooking.TeamId;
                            booking.Comment = newBooking.Comment;
                            booking.IsAvailable = false;
                            context.Bookings.Update(booking);
                            await context.SaveChangesAsync();
                        }
                        else
                        {
                            booking.BookingDate = newBooking.BookingDate;
                            booking.RoomId = newBooking.RoomId;
                            booking.TeamId = newBooking.TeamId;
                            booking.Comment = newBooking.Comment;
                            booking.IsAvailable = false;
                            await context.AddAsync(booking);
                            await context.SaveChangesAsync();
                        }
                        return Ok("Booking successful!");
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
                using (var context = new HouseofmysteriesContext())
                {
                    try
                    {
                        Booking booking = new Booking { BookingId = bookingId };
                        context.Remove(booking);
                        await context.SaveChangesAsync();
                        return Ok("Delete succesful!");
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

        [HttpGet("TeamCompetition/{roomId}")]
        public async Task<ActionResult> TeamCompetition(int roomId, int limit)
        {
            using (var context = new HouseofmysteriesContext())
            {
                try
                {
                    if (limit ==0 || limit ==null) 
                    {
                        limit = 1;
                    }
                    return Ok(context.Bookings.FromSql($"SELECT * FROM `booking` WHERE(result IS NOT null)AND(roomId = {roomId})AND(teamId IS NOT null) ORDER by result LIMIT {limit}").ToList());
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }



    }
}


