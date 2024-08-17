using MediatR;

namespace Restaurants.Application.Restaurants.Commands.NewFolder
{
    public class UpdateRestaurantCommand : IRequest
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public bool HasDelivery { get; set; }
    }
}