using TravelAgency.ViewModels.Models.HotelModels;

namespace TravelAgency.Service.Core.Contracts
{
    public interface IHotelInterface
    {
        Task<IEnumerable<GetAllHotelsViewModel>> GetAllHotelsAsync();

        Task<HotelDetailsViewModel> GetHotelDetailsAsync(string id);

        Task<HotelEditViewModel> GetHotelForEditAsync(string? id);

        Task<bool> SaveEditChangesAsync(HotelEditViewModel? model);
    }
}
