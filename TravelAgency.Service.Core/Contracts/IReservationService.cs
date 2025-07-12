using TravelAgency.ViewModels.Models.ReservationModels;

namespace TravelAgency.Service.Core.Contracts
{
    public interface IReservationService
    {
        Task<IEnumerable<GetAllReservationsViewModel>> GetAllReservationsAsync(string? userId);

        Task<AddReservationViewModel> GetReservationDetailsAsync(string? hotelId);

        Task<bool> AddReservationAsync(string? userId, AddReservationViewModel? model);

        Task RemoveFromFavoritesAsync(string? reservationId);
    }
}
