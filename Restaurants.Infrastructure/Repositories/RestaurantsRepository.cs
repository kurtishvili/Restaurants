using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;
using System.Linq.Expressions;

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

        public async Task<(IEnumerable<Restaurant>, int)> GetAllMatchingAsync(string? searchPhrase, int pageSize, int pageNumber, string? sortBy, SortDirection sortDirection)
        {
            var searchPhraseLower = searchPhrase?.ToLower();

            var baseQuery = dbContext.Restaurants
                .Where(r => searchPhrase == null || (r.Name.ToLower().Contains(searchPhraseLower)
                                                 || r.Description.ToLower().Contains(searchPhraseLower)));

            var totalCount = await baseQuery.CountAsync();

            if (sortBy != null)
            {
                var columnsSelector = new Dictionary<string, Expression<Func<Restaurant, object>>>
                {
                    {nameof(Restaurant.Name), r => r.Name },
                    {nameof(Restaurant.Category), r => r.Category },
                    {nameof(Restaurant.Description), r => r.Description }
                };

                var seelctedColumn = columnsSelector[sortBy];

                baseQuery = sortDirection == SortDirection.Ascending 
                    ? baseQuery.OrderBy(seelctedColumn) 
                    : baseQuery.OrderByDescending(seelctedColumn);
            }

            var restaurants = await baseQuery
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (restaurants, totalCount);
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