using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories
{
    internal class DishesRepository(RestaurantsDbContext dbContext) : IDishesRepository
    {
        public async Task<int> Create(Dish dish, CancellationToken cancellationToken)
        {
            dbContext.Dishes.Add(dish);
            await dbContext.SaveChangesAsync();

            return dish.Id;
        }

        public async Task Delete(IEnumerable<Dish> dishes, CancellationToken cancellation)
        {
            dbContext.Dishes.RemoveRange(dishes);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Dish>> GetForRestaurant(int restaurantId)
        {
            var dishes = await dbContext.Dishes.ToListAsync();

            return dishes;
        }
    }
}