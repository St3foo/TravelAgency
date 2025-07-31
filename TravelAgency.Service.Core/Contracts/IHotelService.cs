using TravelAgency.ViewModels.Models.HotelModels;
using TravelAgency.ViewModels.Models.TourModels;

namespace TravelAgency.Service.Core.Contracts
{
    public interface IHotelService
    {
        Task<IEnumerable<GetAllHotelsViewModel>> GetAllHotelsAsync();

        Task<IEnumerable<GetAllHotelsViewModel>> GetAllHotelsForAdminAsync();

        Task<IEnumerable<GetAllHotelsViewModel>> GetAllHotelsByDestinationIdAsync(string? id);

        Task<HotelDetailsViewModel> GetHotelDetailsAsync(string id);

        Task<HotelEditViewModel> GetHotelForEditAsync(string? id);

        Task<bool> SaveEditChangesAsync(HotelEditViewModel? model);

        Task<bool> AddHotelAsync(AddHotelViewModel? model);

        Task DeleteOrRestoreHotelAsync(string? id);

        Task<IEnumerable<GetAllHotelsForAddTourViewModel>> GetHotelsForTourAsync(string? id);
    }
}
