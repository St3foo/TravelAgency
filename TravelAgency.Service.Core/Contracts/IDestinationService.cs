using TravelAgency.ViewModels.Models.DestinationModels;

namespace TravelAgency.Service.Core.Contracts
{
    public interface IDestinationService
    {
        Task<IEnumerable<AllDestinationsViewModel>> GetAllDestinationsAsync();

        Task<IEnumerable<AllDestinationsViewModel>> GetAllDestinationsForAdminAsync();

        Task<DestinationDetailViewModel> GetDestinationDetailsAsync(string destinationId);

        Task<DestinationEditViewModel> GetDestinationForEditAsync(string destinationId);

        Task<bool> SaveEditChangesAsync(DestinationEditViewModel? model);

        Task<bool> AddDestinationAsync(AddDestinationViewModel? model);

        Task DeleteOrRestoreDestinationAsync(string? id);
    }
}
