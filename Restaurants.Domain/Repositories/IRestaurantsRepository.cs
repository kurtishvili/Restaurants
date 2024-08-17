using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories
{
    public interface IRestaurantsRepository
    {
        Task<IEnumerable<Restaurant>> GetAllAsync();

        Task<Restaurant> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task<int> Create(Restaurant restaurant, CancellationToken cancellationToken);

        Task Delete(Restaurant restaurant, CancellationToken cancellationToken);

        Task SaveChanges();
    }
}
