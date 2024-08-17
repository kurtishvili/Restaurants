using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.RemoveDish
{
    internal class DeleteDishesForRestaurantCommandHandler(
        ILogger<DeleteDishesForRestaurantCommand> logger, 
        IRestaurantsRepository restaurantsRepository,
        IDishesRepository dishesRepository) : IRequestHandler<DeleteDishesForRestaurantCommand>
    {
        public async Task Handle(DeleteDishesForRestaurantCommand request, CancellationToken cancellationToken)
        {
            logger.LogWarning("Removing all dishes from restaurant {RestaurantId}", request.RestaurantId);

            var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId, cancellationToken)
                ?? throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

            await dishesRepository.Delete(restaurant.Dishes, cancellationToken);
        }
    }
}
