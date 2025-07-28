using TravelAgency.ViewModels.Models.TourModels;

namespace TravelAgency.Service.Core.Contracts
{
    public interface ITourService
    {
        Task<IEnumerable<GetAllToursViewModel>> GetAllToursAsync();

        Task<TourDetailsViewModel> GetTourDetailsAsync(string? id);
    }
}
