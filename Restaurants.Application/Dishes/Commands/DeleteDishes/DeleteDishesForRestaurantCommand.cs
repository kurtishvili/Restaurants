using MediatR;

namespace Restaurants.Application.Dishes.Commands.RemoveDish
{
    public class DeleteDishesForRestaurantCommand(int restaurantId) : IRequest
    {
        public int RestaurantId { get; } = restaurantId;
    }
}