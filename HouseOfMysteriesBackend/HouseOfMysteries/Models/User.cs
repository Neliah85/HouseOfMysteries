﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HouseOfMysteries.Models;

public partial class User
{
    public int UserId { get; set; }

    public string RealName { get; set; } = null!;

    public string NickName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;
    public int? TeamId { get; set; }
    public int? RoleId { get; set; }

    public string Salt { get; set; } = null!;

    public string Hash { get; set; } = null!;
    [JsonIgnore]
    public virtual Role? Role { get; set; }
    [JsonIgnore]
    public virtual Team? Team { get; set; }
}
