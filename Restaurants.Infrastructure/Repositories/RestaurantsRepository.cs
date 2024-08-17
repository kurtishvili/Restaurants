using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories
{
    internal class RestaurantsRepository(RestaurantsDbContext dbContext) : IRestaurantsRepository
    {
        public async Task<int> Create(Restaurant restaurant, CancellationToken cancellationToken)
        {
            dbContext.Restaurants.Add(restaurant);
            await dbContext.SaveChangesAsync();

            return restaurant.Id;
        }

        public async Task Delete(Restaurant restaurant, CancellationToken cancellationToken)
        {
            dbContext.Remove(restaurant);

            await dbContext.SaveChangesAsync();
        }


        public async Task<IEnumerable<Restaurant>> GetAllAsync()
        {
            var restaurants = await dbContext.Restaurants.ToListAsync();

            return restaurants;
        }

        public async Task<Restaurant?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var restaurant = await dbContext.Restaurants.Include(r => r.Dishes).FirstOrDefaultAsync(r => r.Id == id);

            return restaurant;
        }

        public async Task Update(Restaurant restaurant, CancellationToken cancellationToken)
        {
            dbContext.Update(restaurant);

            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task SaveChanges() => dbContext.SaveChangesAsync();
    }
}