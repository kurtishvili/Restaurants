using MediatR;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant
{
    public class CreateRestaurantCommand : IRequest<int>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public bool HasDelivery { get; set; }
        // contact

        public string ContactEmail { get; set; }

        public string ContactNumber { get; set; }
        // address
        public string City { get; set; }

        public string Street { get; set; }

        public string PostalCode { get; set; }
    }
}