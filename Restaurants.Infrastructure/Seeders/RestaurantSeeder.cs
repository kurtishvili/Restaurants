using Microsoft.AspNetCore.Identity;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Seeders
{
    internal class RestaurantSeeder(RestaurantsDbContext dbContext) : IRestaurantSeeder
    {
        public async Task Seed()
        {
            if (await dbContext.Database.CanConnectAsync())
            {
                if (!dbContext.Restaurants.Any())
                {
                    var restarants = GetRestaurants();
                    dbContext.Restaurants.AddRange(restarants);
                    await dbContext.SaveChangesAsync();
                }
                if (!dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    dbContext.Roles.AddRange(roles);
                    await dbContext.SaveChangesAsync();
                }
            }
        }

        private IEnumerable<IdentityRole> GetRoles()
        {
            List<IdentityRole> roles = [
                new (UserRoles.User){
                    NormalizedName = UserRoles.User.ToUpper(),
                },
                new (UserRoles.Owner){
                    NormalizedName = UserRoles.Owner.ToUpper(),
                },
                new (UserRoles.Admin){
                    NormalizedName = UserRoles.Admin.ToUpper(),
                },
                ];

            return roles;
        }

        private IEnumerable<Restaurant> GetRestaurants()
        {
            List<Restaurant> restaurants = [
                new(){
                    Name = "KFC",
                    Category = "Fast Food",
                    Description ="KFC (short for kentucky fried chicken) is an American fast food restaurant",
                    HasDelivery = true,
                    Dishes = [
                        new(){
                            Name ="Nashville hot chicken",
                            Description = "nashville hot chicken (10 pcs)",
                            Price = 5
                        },
                        new(){
                            Name ="chicken burger",
                            Description = "burger with potato",
                            Price = 10
                        }
                        ],
                    Address = new(){
                        City = "Tbilisi",
                        Street = "Vaja",
                        PostalCode ="1234"
                    }
                },
                new(){
                    Name = "McDonald",
                    Category = "Fast Food",
                    Description ="McDonald is an American fast food restaurant",
                    HasDelivery = true,
                    Address = new(){
                        City = "Tbilisi",
                        Street = "Saburatlo",
                        PostalCode ="12345"
                    }
                },
                ];

            return restaurants;
        }
    }
}