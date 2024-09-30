using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Queries.GetDishesForRestaurant
{
    internal class GetDishesForRestaurantQueryHandler(
        ILogger<GetDishesForRestaurantQueryHandler> logger,
        IMapper mapper,
        IRestaurantsRepository restaurantsRepository,
        IDishesRepository dishesRepository) : IRequestHandler<GetDishesForRestaurantQuery, IEnumerable<DishDto>>
    {
        public async Task<IEnumerable<DishDto>> Handle(GetDishesForRestaurantQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Retrieving dishes for restaurant with id: {RestaurantId}", request.RestaunratId);

            var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaunratId, cancellationToken)
                ?? throw new NotFoundException(nameof(Restaurant), request.RestaunratId.ToString());

            var result = mapper.Map<IEnumerable<DishDto>>(restaurant.Dishes);

            return result;
        }
    }
}