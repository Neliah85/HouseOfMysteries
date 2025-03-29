using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceToolWPF.Classes
{
    public class SaveResultDTO
    {
        public DateTime BookingDate { get; set; }
        public int RoomId { get; set; }
        public TimeSpan Result { get; set; }
    }
}
