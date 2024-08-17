using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories
{
    public interface IDishesRepository
    {
        Task<int> Create(Dish dish, CancellationToken cancellationToken);

        Task Delete(IEnumerable<Dish> dishes, CancellationToken cancellation);

        Task<IEnumerable<Dish>> GetForRestaurant(int restaurantId);
    }
}