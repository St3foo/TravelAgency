using TravelAgency.ViewModels.Models.HotelModels;

namespace TravelAgency.Service.Core.Contracts
{
    public interface IHotelInterface
    {
        Task<IEnumerable<GetAllHotelsViewModel>> GetAllHotelsAsync();

        Task<IEnumerable<GetAllHotelsViewModel>> GetAllHotelsByDestinationIdAsync(string? id);

        Task<HotelDetailsViewModel> GetHotelDetailsAsync(string id);

        Task<HotelEditViewModel> GetHotelForEditAsync(string? id);

        Task<bool> SaveEditChangesAsync(HotelEditViewModel? model);

        Task<bool> AddHotelAsync(AddHotelViewModel? model);

        Task<DeleteHotelViewModel> GetHotelForDeleteAsync(string? id);

        Task<bool> DeleteHotelAsync(DeleteHotelViewModel? model);
    }
}
