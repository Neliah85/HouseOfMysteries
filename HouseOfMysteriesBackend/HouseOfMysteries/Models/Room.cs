using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HouseOfMysteries.Models;

public partial class Room
{
    public int RoomId { get; set; }

    public string RoomName { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
