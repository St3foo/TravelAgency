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

        public async Task<bool> DeleteDestinationAsync(DeleteDestinationViewModel? model)
        {
            bool result = false;

            Destination? destination = await _destinationRepository
                .SingleOrDefaultAsync(d => d.Id == model.Id);

            if (destination != null)
            {
                result = await _destinationRepository.DeleteAsync(destination);
            }

            return result;
        }

        public async Task<IEnumerable<AllDestinationsViewModel>> GetAllDestinationsAsync()
        {
            IEnumerable<AllDestinationsViewModel> destinations = await _destinationRepository
                .GetAllAttached()
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

        public async Task<DeleteDestinationViewModel> GetDestinationForDeleteAsync(string? destinationId)
        {
            DeleteDestinationViewModel? destinationToPass = null;

            bool isValidGuid = Guid.TryParse(destinationId, out Guid id);

            if (isValidGuid)
            {
                destinationToPass = await _destinationRepository
                    .GetAllAttached()
                    .AsNoTracking()
                    .Where(d => d.Id == id)
                    .Select(d => new DeleteDestinationViewModel
                    {
                        Id = d.Id,
                        Name = d.CountryName,
                        ImageUrl = d.ImageUrl
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
