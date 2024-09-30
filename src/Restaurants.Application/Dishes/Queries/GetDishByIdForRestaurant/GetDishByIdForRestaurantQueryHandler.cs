using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Queries.GetDishByIdForRestaurant
{
    internal class GetDishByIdForRestaurantQueryHandler(
        ILogger<GetDishByIdForRestaurantQueryHandler> logger,
        IRestaurantsRepository restaurantsRepository,
        IMapper mapper) : IRequestHandler<GetDishByIdForRestaurantQuery, DishDto>
    {
        public async Task<DishDto> Handle(GetDishByIdForRestaurantQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Retrieving Dish {DishId} for restaurant with id: {RestaurantId}", request.DishId, request.RestaurantId);

            var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId, cancellationToken)
                ?? throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

            var dish = restaurant.Dishes.FirstOrDefault(d => d.Id == request.DishId)
                ?? throw new NotFoundException(nameof(Dish), request.DishId.ToString());

            var dishDto = mapper.Map<DishDto>(dish);

            return dishDto;
        }
    }
}