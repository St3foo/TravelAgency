using TravelAgency.ViewModels.Models.ReservationModels;

namespace TravelAgency.Service.Core.Contracts
{
    public interface IReservationService
    {
        Task<IEnumerable<GetUserReservationsViewModel>> GetUserReservationsAsync(string? userId);

        Task<IEnumerable<GetAllReservationViewModel>> GetAllReservationsAsync();

        Task<AddReservationViewModel> GetReservationDetailsAsync(string? hotelId);

        Task<bool> AddReservationAsync(string? userId, AddReservationViewModel? model);

        Task RemoveFromReservationAsync(string? reservationId);
    }
}
