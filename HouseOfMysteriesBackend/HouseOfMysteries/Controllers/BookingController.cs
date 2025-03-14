using HouseOfMysteries.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HouseOfMysteries.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        [HttpPost("token,bookingTime,roomId,teamId,comment")]
        public async Task<IActionResult>Post(string token, DateTime bookingDate, int roomId, int? teamId, string comment)
        {
            using (var context = new HouseofmysteriesContext())
                try
                {
                    if (context.Bookings.FirstOrDefaultAsync(f => f.BookingDate == bookingDate && f.IsAvailable == false) != null)
                    {
                        return Ok("A szoba nem foglalható erre az időpontra!");
                    }



                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
        }
    }
}
