﻿namespace TravelAgency.ViewModels.Models.HotelModels
{
    public class DeleteHotelViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string? ImageUrl { get; set; }
    }
}
