using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceToolWPF.Classes
{
    internal class CheckBookingDTO
    {
        public string Token { get; set; }
        public DateTime BookingDate { get; set; }         
        public int RoomId { get; set; } 
    }
}
