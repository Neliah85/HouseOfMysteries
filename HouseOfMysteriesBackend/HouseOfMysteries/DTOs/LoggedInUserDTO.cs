using HouseOfMysteries.Models;

namespace HouseOfMysteries.DTOs
{
    public class LoggedInUserDTO
    {

        public string Token { get; set; } = null!;
        public int UserId { get; set; }

        public string RealName { get; set; } = null!;

        public string NickName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public int? TeamId { get; set; }

        public int? RoleId { get; set; }

        public virtual Role? Role { get; set; }

        public virtual Team? Team { get; set; }
    }
}
