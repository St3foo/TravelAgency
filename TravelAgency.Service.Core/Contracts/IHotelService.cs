using TravelAgency.ViewModels.Models.HotelModels;
using TravelAgency.ViewModels.Models.TourModels;

namespace TravelAgency.Service.Core.Contracts
{
    public interface IHotelService
    {
        Task<IEnumerable<GetAllHotelsViewModel>> GetAllHotelsAsync(string? search);

        Task<IEnumerable<GetAllHotelsViewModel>> GetAllHotelsForAdminAsync(string? search);

        Task<IEnumerable<GetAllHotelsViewModel>> GetAllHotelsByDestinationIdAsync(string? id, string? search);

        Task<HotelDetailsViewModel> GetHotelDetailsAsync(string id);

        Task<HotelEditViewModel> GetHotelForEditAsync(string? id);

        Task<bool> SaveEditChangesAsync(HotelEditViewModel? model);

        Task<bool> AddHotelAsync(AddHotelViewModel? model);

        Task DeleteOrRestoreHotelAsync(string? id);

        Task<IEnumerable<GetAllHotelsForAddTourViewModel>> GetHotelsForTourAsync(string? id);
    }
}
