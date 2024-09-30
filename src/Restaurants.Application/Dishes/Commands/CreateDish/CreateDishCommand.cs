using MediatR;

namespace Restaurants.Application.Dishes.Commands.CreateDish
{
    public class CreateDishCommand : IRequest<int>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        public int? KiloCalories { get; set; }

        public int RestaurantId { get; set; }
    }
}