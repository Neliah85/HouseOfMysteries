﻿namespace HouseOfMysteries.DTOs
{
    public class SaveResultDTO
    {
        public DateTime BookingDate { get; set; }
        public int RoomId { get; set; } 
        public TimeSpan Result {  get; set; }
    }
}
