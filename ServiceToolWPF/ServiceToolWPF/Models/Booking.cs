﻿namespace ServiceToolWPF.Models;
public partial class Booking
{
    public int BookingId { get; set; }

    public DateTime? BookingDate { get; set; }

    public int? RoomId { get; set; }

    public int? TeamId { get; set; }

    public TimeSpan? Result { get; set; }

    public bool? IsAvailable { get; set; }

    public string? Comment { get; set; }

}
