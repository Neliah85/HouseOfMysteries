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
                        if (context.Bookings.FirstOrDefaultAsync(f => f.BookingDate.Equals(clearBooking.BookingDate) && f.RoomId == clearBooking.RoomId && f.IsAvailable == false).Result != null)
                        {
                            booking = context.Bookings.FirstOrDefaultAsync(f => f.BookingDate.Equals(clearBooking.BookingDate) && f.RoomId == clearBooking.RoomId && f.IsAvailable == false).Result;
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
                        if (context.Bookings.FirstOrDefaultAsync(f => f.BookingDate.Equals(newBooking.BookingDate) && f.RoomId == newBooking.RoomId && f.IsAvailable == false).Result != null)
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
        public async Task<ActionResult> TeamCompetition(int roomId)
        {
            using (var context = new HouseofmysteriesContext())
            {
                try
                {
                    //return Ok(context.Bookings.FromSql($"SELECT teams.teamName AS Csapat neve, MIN(booking.result) AS Legjobb idő FROM booking JOIN  rooms ON booking.roomId = rooms.roomId JOI  teams ON booking.teamId = teams.teamId WHERE booking.result IS NOT NULL --Csak a befejezett játékokat veszem figyelembe GROUP BY rooms.roomId, teams.teamId ORDER BY rooms.roomName, MIN(booking.result);").ToList());
                    
                    return Ok(context.Bookings.FromSql($"SELECT r.roomName, t.teamName, b.result FROM booking b JOIN rooms r ON b.roomId = r.roomId JOIN teams t ON  b.teamId = t.teamId WHERE b.roomId = 1 ORDER BY b.result ASC;").ToList());




                    //return Ok(context.Bookings.FromSql($"SELECT ranked.roomId, t.teamName, ranked.result FROM (SELECT booking.roomId,booking.teamId,booking.result,RANK() OVER (PARTITION BY booking.roomId ORDER BY booking.result ASC) AS ranking FROM booking WHERE booking.result IS NOT NULL) AS ranked JOIN teams t ON ranked.teamId = t.teamId WHERE ranked.ranking <= 3 ORDER BY ranked.roomId, ranked.result;").ToList());
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }




    }
}


