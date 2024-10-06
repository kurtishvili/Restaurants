using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.GetRestaurantById
{
    internal class GetRestaurantByIdQueryHandler(
        ILogger<GetRestaurantByIdQueryHandler> logger,
        IMapper mapper,
        IBlobStorageService blobStorageService,
        IRestaurantsRepository restaurantsRepository) : IRequestHandler<GetRestaurantByIdQuery, RestaurantDto>
    {
        public async Task<RestaurantDto> Handle(GetRestaurantByIdQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Getting restaurant {RestaurantId}", request.Id);

            var restaurant = await restaurantsRepository.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(Restaurant), request.Id.ToString());

            var restaurantDto = mapper.Map<RestaurantDto>(restaurant);

            restaurantDto.LogoSasUrl = blobStorageService.GetBlobSasUrl(restaurant.LogoUrl);

            return restaurantDto;
        }
    }
}