using TravelAgency.ViewModels.Models.DestinationModels;

namespace TravelAgency.Service.Core.Contracts
{
    public interface IDestinationService
    {
        Task<IEnumerable<AllDestinationsViewModel>> GetAllDestinationsAsync();
    }
}
