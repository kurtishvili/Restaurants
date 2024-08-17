﻿using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant
{
    public class CreateRestaurantCommandHandler(
        IRestaurantsRepository restaurantsRepository, 
        ILogger<CreateRestaurantCommandHandler> logger, 
        IMapper mapper) 
        : IRequestHandler<CreateRestaurantCommand, int>
    {
        public async Task<int> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Creating a new Restaurant {@Restaurant}", request);

            var restaurant = mapper.Map<Restaurant>(request);

            var id = await restaurantsRepository.Create(restaurant, cancellationToken);

            return id;
        }
    }
}