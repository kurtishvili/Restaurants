using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.UploadRestaurantLogo
{
    public class UploadRestaurantLogoCommandHandler(ILogger<UploadRestaurantLogoCommandHandler> logger,
        IRestaurantsRepository restaurantsRepository,
        IRestaurantAuthorizationService restaurantAuthorizationService,
        IBlobStorageService blobStorageService
        ) : IRequestHandler<UploadRestaurantLogoCommand>
    {
        public async Task Handle(UploadRestaurantLogoCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("uploading restaurnt logo for id: {@RestaunratId}", request.RestaunratId);

            var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaunratId, cancellationToken);

            if (restaurant == null)
                throw new NotFoundException(nameof(Restaurant), request.RestaunratId.ToString());

            if (!restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Update))
                throw new ForbidException();

            var logoUrl = await blobStorageService.UploadToBlobAsync(request.File, request.FileName);

            restaurant.LogoUrl = logoUrl;

            await restaurantsRepository.SaveChanges();
        }
    }
}
