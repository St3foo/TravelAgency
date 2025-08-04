using Microsoft.EntityFrameworkCore;
using TravelAgency.Data;
using TravelAgency.Data.Models;
using TravelAgency.Data.Repository.Interfaces;
using TravelAgency.Service.Core.Contracts;
using TravelAgency.ViewModels.Models.DestinationModels;

namespace TravelAgency.Service.Core
{
    public class DestinationService : IDestinationService
    {
        private readonly IDestinationRepository _destinationRepository;

        public DestinationService(IDestinationRepository destinationRepository)
        {
            _destinationRepository = destinationRepository;
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

                await _destinationRepository.AddAsync(destination);

                result = true;
            }

            return result;
        }

        public async Task DeleteOrRestoreDestinationAsync(string? id)
        {
            if (!String.IsNullOrWhiteSpace(id))
            {
                Destination? destination = await _destinationRepository
                    .GetAllAttached()
                    .IgnoreQueryFilters()
                    .SingleOrDefaultAsync(d => d.Id.ToString().ToLower() == id.ToLower());
                if (destination != null)
                {
                    destination.IsDeleted = !destination.IsDeleted;

                    await _destinationRepository
                        .UpdateAsync(destination);
                }
            }
        }

        public async Task<IEnumerable<AllDestinationsViewModel>> GetAllDestinationsAsync(string? search)
        {
            IEnumerable<AllDestinationsViewModel> destinations = await _destinationRepository
                .GetAllAttached()
                .AsNoTracking()
                .Select(d => new AllDestinationsViewModel
                {
                    Id = d.Id.ToString(),
                    Name = d.CountryName,
                    ImageUrl = d.ImageUrl,
                    IsDeleted = d.IsDeleted
                })
                .ToArrayAsync();

            if (!String.IsNullOrEmpty(search))
            {
                return destinations.Where(d => d.Name.Contains(search));
            }

            return destinations;
        }

        public async Task<IEnumerable<AllDestinationsViewModel>> GetAllDestinationsForAdminAsync(string? search)
        {
            IEnumerable<AllDestinationsViewModel> destinations = await _destinationRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Select(d => new AllDestinationsViewModel
                {
                    Id = d.Id.ToString(),
                    Name = d.CountryName,
                    ImageUrl = d.ImageUrl,
                    IsDeleted = d.IsDeleted
                })
                .ToArrayAsync();

            if (!String.IsNullOrEmpty(search))
            {
                destinations = destinations.Where(d => d.Name.Contains(search));
            }

            return destinations;
        }

        public async Task<DestinationDetailViewModel> GetDestinationDetailsAsync(string destinationId)
        {
            DestinationDetailViewModel? destinationToPass = null;

            bool isValidGuid = Guid.TryParse(destinationId, out Guid id);

            if (isValidGuid)
            {
                destinationToPass = await _destinationRepository
                    .GetAllAttached()
                    .AsNoTracking()
                    .Where(d => d.Id == id)
                    .Select(d => new DestinationDetailViewModel
                    {
                        Id = d.Id.ToString(),
                        Title = d.CountryName,
                        ImageUrl = d.ImageUrl,
                        Description = d.Description
                    })
                    .SingleOrDefaultAsync();
            }

            return destinationToPass;
        }

        public async Task<DestinationEditViewModel> GetDestinationForEditAsync(string id)
        {
            DestinationEditViewModel? destinationToPass = null;

            bool isValidGuid = Guid.TryParse(id, out Guid destId);

            if (isValidGuid)
            {
                destinationToPass = await _destinationRepository
                    .GetAllAttached()
                    .IgnoreQueryFilters()
                    .AsNoTracking()
                    .Where(d => d.Id == destId)
                    .Select(d => new DestinationEditViewModel
                    {
                        Id = d.Id.ToString(),
                        Name = d.CountryName,
                        Description = d.Description,
                        ImageUrl = d.ImageUrl
                    })
                    .SingleOrDefaultAsync();
            }

            return destinationToPass;
        }

        public async Task<bool> SaveEditChangesAsync(DestinationEditViewModel? model)
        {
            bool result = false;

            if (model != null)
            {
                var destination = await _destinationRepository
                    .GetAllAttached()
                    .IgnoreQueryFilters()
                    .SingleOrDefaultAsync(d => d.Id.ToString() == model.Id);

                if (destination != null)
                {
                    destination.CountryName = model.Name;
                    destination.Description = model.Description;
                    destination.ImageUrl = model.ImageUrl;

                    result = await _destinationRepository.UpdateAsync(destination);
                }
            }

            return result;
        }
    }
}
