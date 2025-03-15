using HouseOfMysteries.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Specialized;
using System.Globalization;

namespace HouseOfMysteries.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        [HttpPut("ClearBooking/{token}")]
        public async Task<IActionResult>ClearBooking(string token, DateTime bookingDate, int roomId)
        {
            if (Program.loggedInUsers.CheckTokenValidity(token).LoggedInUser.RoleId > 1)
            {
                Booking? booking = new Booking();
                using (var context = new HouseofmysteriesContext())
                {
                    try
                    {
                        if (context.Bookings.FirstOrDefaultAsync(f => f.BookingDate.Equals(bookingDate) && f.RoomId == roomId && f.IsAvailable == false).Result != null)
                        {
                            booking = context.Bookings.FirstOrDefaultAsync(f => f.BookingDate.Equals(bookingDate) && f.RoomId == roomId && f.IsAvailable == false).Result;
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
            }
            else
            {
                return BadRequest("You do not have permission to perform this operation!");
            }
        }

        [HttpPut("SaveResult/{token}")]
        public async Task<IActionResult> SaveResult(string token, DateTime bookingDate, int roomId, TimeSpan result)
        {
            if (Program.loggedInUsers.CheckTokenValidity(token).LoggedInUser.RoleId > 2)
            {
                Booking? booking = new Booking();
                using (var context = new HouseofmysteriesContext())
                {
                    try
                    {
                        if (context.Bookings.FirstOrDefaultAsync(f => f.BookingDate.Equals(bookingDate) && f.RoomId == roomId).Result != null)
                        {
                            booking = context.Bookings.FirstOrDefaultAsync(f => f.BookingDate.Equals(bookingDate) && f.RoomId == roomId).Result;
                            booking.Result = result;
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


        [HttpPost("{token}")]
        public async Task<IActionResult> Post(string token, DateTime bookingDate, int roomId, int? teamId, string? comment)
        {
            if (Program.loggedInUsers.CheckTokenValidity(token).LoggedInUser.RoleId > 1)
            {
                Booking? booking = new Booking();
                using (var context = new HouseofmysteriesContext())
                {
                    try
                    {
                        if (context.Bookings.FirstOrDefaultAsync(f => f.BookingDate.Equals(bookingDate) && f.RoomId == roomId && f.IsAvailable == false).Result != null)
                        {
                            return Ok("The room cannot be booked for this time!");
                        }

                        if (context.Bookings.FirstOrDefaultAsync(f => f.BookingDate.Equals(bookingDate) && f.RoomId == roomId && f.IsAvailable == true).Result != null)
                        {
                            booking = context.Bookings.FirstOrDefaultAsync(f => f.BookingDate.Equals(bookingDate) && f.RoomId == roomId && f.IsAvailable == true).Result;
                            booking.TeamId = teamId;
                            booking.Comment = comment;
                            booking.IsAvailable = false;
                            context.Bookings.Update(booking);
                            await context.SaveChangesAsync();
                        }
                        else
                        {
                            booking.BookingDate = bookingDate;
                            booking.RoomId = roomId;
                            booking.TeamId = teamId;
                            booking.Comment = comment;
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

        [HttpGet("{token}")]

        public IActionResult Get(string token, DateTime day, int roomId)
        {
            if (Program.loggedInUsers.CheckTokenValidity(token).LoggedInUser.RoleId > 1)
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


        [HttpDelete("{token}")]

        public async Task<IActionResult> Delete(string token, int bookingId)
        {
            if (Program.loggedInUsers.CheckTokenValidity(token).LoggedInUser.RoleId > 3)
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
    }
}