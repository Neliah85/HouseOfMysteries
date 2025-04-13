﻿using System.Text.Json.Serialization;

namespace HouseOfMysteries.Models;
public partial class Role
{
    public int RoleId { get; set; }
    public string RoleName { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<User>? Users { get; set; } = new List<User>();
}
