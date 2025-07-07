using TravelAgency.ViewModels.Models.HotelModels;

namespace TravelAgency.Service.Core.Contracts
{
    public interface IHotelInterface
    {
        Task<IEnumerable<GetAllHotelsViewModel>> GetAllHotelsAsync();
    }
}
