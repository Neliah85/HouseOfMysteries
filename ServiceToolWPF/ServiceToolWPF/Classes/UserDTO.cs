using System;
using System.Collections.Generic;
using System.Data;

namespace ServiceToolWPF.Classes;

public partial class UserDTO
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


}
