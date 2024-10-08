﻿using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.CreateDish
{
    public class CreateDishCommandHandler(
        IMapper mapper,
        ILogger<CreateDishCommandHandler> logger,
        IRestaurantsRepository restaurantsRepository,
        IDishesRepository dishesRepository,
        IRestaurantAuthorizationService restaurantAuthorizationService)
        : IRequestHandler<CreateDishCommand, int>
    {

        public async Task<int> Handle(CreateDishCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Creating new dish: {@DishRequest}", request);

            var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId, cancellationToken)
                ?? throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
            var dish = mapper.Map<Dish>(request);

            if (!restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Update))
                throw new ForbidException();

            var dishId = await dishesRepository.Create(dish, cancellationToken);

            return dishId;
        }
    }
}