using System;
using System.Collections.Generic;

namespace HouseOfMysteries.Models;

public partial class Team
{
    public int TeamId { get; set; }

    public string TeamName { get; set; } = null!;

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
