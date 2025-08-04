using TravelAgency.ViewModels.Models.TourModels;

namespace TravelAgency.Service.Core.Contracts
{
    public interface ITourService
    {
        Task<IEnumerable<GetAllToursViewModel>> GetAllToursAsync(string? search);

        Task<IEnumerable<GetAllToursViewModel>> GetAllToursForAdminAsync(string? search);

        Task<TourDetailsViewModel> GetTourDetailsAsync(string? id);

        Task DeleteOrRestoreTourAsync(string? id);

        Task<bool> AddTourAsync(AddTourViewModel? model);

        Task<TourEditViewModel> GetTourForEditAsync(string? id);

        Task<bool> SaveEditChangesAsync(TourEditViewModel? model);
    }
}
