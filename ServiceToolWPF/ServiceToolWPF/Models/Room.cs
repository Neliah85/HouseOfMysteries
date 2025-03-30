using System;
using System.Collections.Generic;

namespace ServiceToolWPF.Models;
public partial class Room
{
    public int RoomId { get; set; }

    public string RoomName { get; set; } = null!;

   // public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
