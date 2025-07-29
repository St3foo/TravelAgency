using TravelAgency.ViewModels.Models.Book;

namespace TravelAgency.Service.Core.Contracts
{
    public interface IBookService
    {
        Task<IEnumerable<GetUserBookingsViewModel>> GetUserBookingsAsync(string? userId);

        Task<IEnumerable<GetAllBookingsViewModel>> GetAllBookingsAsync();

        Task<AddBookingViewModel> GetBookingDetailsAsync(string? tourId);

        Task<bool> AddBookingAsync(string? userId, AddBookingViewModel? model);

        Task RemoveBookingAsync(string? id);
    }
}
