namespace TravelAgency.ViewModels.Models.TourModels
{
    public class GetAllHotelsForAddTourViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public int DaysStay { get; set; }
    }
}
