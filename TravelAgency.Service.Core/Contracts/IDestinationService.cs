using TravelAgency.ViewModels.Models.DestinationModels;

namespace TravelAgency.Service.Core.Contracts
{
    public interface IDestinationService
    {
        Task<IEnumerable<AllDestinationsViewModel>> GetAllDestinationsAsync();

        Task<DestinationDetailViewModel> GetDestinationDetailsAsync(string destinationId);

        Task<DestinationEditViewModel> GetDestinationForEditAsync(string destinationId);

        Task SaveEditChangesAsync(DestinationEditViewModel? model);
    }
}
