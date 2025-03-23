namespace HouseOfMysteries.DTOs
{
    public class BookingDTO
    {
        public DateTime BookingDate { get; set; }
        public int RoomId { get; set; }
        public int? TeamId { get; set; } 
        public string Comment { get; set; } = null!;
    }
}
