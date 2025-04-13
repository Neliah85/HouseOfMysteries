using System.Text.Json.Serialization;

namespace HouseOfMysteries.Models;
public partial class Team
{
    public int? TeamId { get; set; }
    public string TeamName { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<Booking>? Bookings { get; set; } = new List<Booking>();
    [JsonIgnore]
    public virtual ICollection<User>? Users { get; set; } = new List<User>();
}
