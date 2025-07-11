using Microsoft.EntityFrameworkCore;
using TravelAgency.Data;
using TravelAgency.Data.Models;
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

        public async Task<bool> AddDestinationAsync(AddDestinationViewModel? model)
        {
            bool result = false;

            if (model != null) 
            {
                Destination destination = new Destination 
                {
                    CountryName = model.Name,
                    Description = model.Description,
                    ImageUrl = model.ImageUrl
                };

                await _context.Destinations.AddAsync(destination);
                await _context.SaveChangesAsync();

                result = true;
            }

            return result;
        }

        public async Task<bool> DeleteDestinationAsync(DeleteDestinationViewModel? model)
        {
            bool result = false;

            Destination? destination = await _context
                .Destinations
                .SingleOrDefaultAsync(d => d.Id == model.Id);

            if (destination != null)
            {
                destination.IsDeleted = true;

                await _context.SaveChangesAsync();
                result = true;
            }

            return result;
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

        public async Task<DeleteDestinationViewModel> GetDestinationForDeleteAsync(string? destinationId)
        {
            DeleteDestinationViewModel? destinationToPass = null;

            var destination = await _context
                .Destinations
                .AsNoTracking()
                .SingleOrDefaultAsync(d => d.Id.ToString() == destinationId);

            if (destination != null)
            {
                destinationToPass = new DeleteDestinationViewModel
                {
                    Id = destination.Id,
                    Name = destination.CountryName,
                    ImageUrl = destination.ImageUrl
                };
            }

            return destinationToPass;
        }

        public async Task<DestinationEditViewModel> GetDestinationForEditAsync(string id)
        {
            DestinationEditViewModel? destinationToPass = null;

            var destination = await _context
                .Destinations
                .AsNoTracking()
                .SingleOrDefaultAsync(d => d.Id.ToString() == id);

            if (destination != null)
            {
                destinationToPass = new DestinationEditViewModel
                {
                    Id = destination.Id.ToString(),
                    Name = destination.CountryName,
                    Description = destination.Description,
                    ImageUrl = destination.ImageUrl
                };
            }

            return destinationToPass;
        }

        public async Task<bool> SaveEditChangesAsync(DestinationEditViewModel? model)
        {
            bool result = false;

            if (model != null) 
            {
                var destination = await _context
                    .Destinations
                    .SingleOrDefaultAsync(d => d.Id.ToString() == model.Id);

                if (destination != null)
                {
                    destination.CountryName = model.Name;
                    destination.Description = model.Description;
                    destination.ImageUrl = model.ImageUrl;

                    await _context.SaveChangesAsync();
                    result = true;
                }
            }

            return result;
        }
    }
}
