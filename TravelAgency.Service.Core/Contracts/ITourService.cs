using TravelAgency.ViewModels.Models.TourModels;

namespace TravelAgency.Service.Core.Contracts
{
    public interface ITourService
    {
        Task<IEnumerable<GetAllToursViewModel>> GetAllToursAsync();

        Task<IEnumerable<GetAllToursViewModel>> GetAllToursForAdminAsync();

        Task<TourDetailsViewModel> GetTourDetailsAsync(string? id);

        Task DeleteOrRestoreTourAsync(string? id);

        Task AddTourAsync(AddTourViewModel? model);
    }
}
