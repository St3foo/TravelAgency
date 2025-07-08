using Microsoft.EntityFrameworkCore;
using TravelAgency.Data;
using TravelAgency.Service.Core.Contracts;
using TravelAgency.ViewModels.Models.DestinationModels;

namespace TravelAgency.Service.Core
{
    public class DestinationService : IDestinationService
    {
        private readonly TravelAgencyDbContext _context;

        public DestinationService(TravelAgencyDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<AllDestinationsViewModel>> GetAllDestinationsAsync()
        {
            IEnumerable<AllDestinationsViewModel> destinations = await _context
                .Destinations
                .AsNoTracking()
                .Select(d => new AllDestinationsViewModel 
                {
                    Id = d.Id.ToString(),
                    Name = d.CountryName,
                    ImageUrl = d.ImageUrl
                })
                .ToArrayAsync();

            return destinations;
        }

        public async Task<DestinationDetailViewModel> GetDestinationDetailsAsync(string destinationId)
        {
            DestinationDetailViewModel? destinationToPass = null;
            
            var destination = await _context
                .Destinations
                .AsNoTracking()
                .SingleOrDefaultAsync(d => d.Id.ToString() == destinationId);

            if (destination != null)
            {
                destinationToPass = new DestinationDetailViewModel
                {
                    Id = destination.Id.ToString(),
                    Title = destination.CountryName,
                    ImageUrl = destination.ImageUrl,
                    Description = destination.Description
                };
            }

            return destinationToPass;
        }
    }
}
