using AutoMapper;
using FluentAssertions;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Xunit;

namespace Restaurants.Application.Tests.Restaurants.Dtos;

public class RestaurantsProfileTests
{
    private IMapper _mapper;

    public RestaurantsProfileTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<RestaurantsProfile>();
        });

        _mapper = configuration.CreateMapper();
    }

    [Fact()]
    public void CreateMap_ForRestaurantToRestaurantDto_MapsCorrectly()
    {
        // arrange

        var restaurant = new Restaurant()
        {
            Id = 1,
            Name = "Test restaurant",
            Description = "Test descrtiption",
            Category = "Test Category",
            HasDelivery = true,
            ContactEmail = "test@example.com",
            ContactNumber = "123456789",
            Address = new Address()
            {
                City = "Test city",
                Street = "Test Street",
                PostalCode = "12345"
            }
        };

        // act

        var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);

        // assert

        restaurantDto.Should().NotBeNull();
        restaurantDto.Id.Should().Be(restaurant.Id);
        restaurantDto.Name.Should().Be(restaurant.Name);
        restaurantDto.Description.Should().Be(restaurant.Description);
        restaurantDto.Category.Should().Be(restaurant.Category);
        restaurantDto.HasDelivery.Should().Be(restaurant.HasDelivery);
        restaurantDto.City.Should().Be(restaurant.Address.City);
        restaurantDto.Street.Should().Be(restaurant.Address.Street);
        restaurantDto.PostalCode.Should().Be(restaurant.Address.PostalCode);
    }

    [Fact()]
    public void CreateMap_ForCreateRestaurantCommandToRestaurant_MapsCorrectly()
    {
        // arrange

        var command = new CreateRestaurantCommand()
        {
            Name = "Test restaurant",
            Description = "Test descrtiption",
            Category = "Test Category",
            HasDelivery = true,
            ContactEmail = "test@example.com",
            ContactNumber = "123456789",
            City = "Test city",
            Street = "Test Street",
            PostalCode = "12345"

        };

        // act

        var restaurant = _mapper.Map<Restaurant>(command);

        // assert

        command.Should().NotBeNull();
        command.Name.Should().Be(restaurant.Name);
        command.Description.Should().Be(restaurant.Description);
        command.Category.Should().Be(restaurant.Category);
        command.HasDelivery.Should().Be(restaurant.HasDelivery);
        command.City.Should().Be(restaurant.Address.City);
        command.Street.Should().Be(restaurant.Address.Street);
        command.PostalCode.Should().Be(restaurant.Address.PostalCode);
    }

    [Fact()]
    public void CreateMap_ForUpdateRestaurantCommandToRestaurant_MapsCorrectly()
    {
        // arrange

        var command = new UpdateRestaurantCommand()
        {
            Id = 1,
            Name = "Test restaurant",
            Description = "Test descrtiption",
            Category = "Test Category",
            HasDelivery = true
        };

        // act

        var restaurant = _mapper.Map<Restaurant>(command);

        // assert

        command.Should().NotBeNull();
        command.Id.Should().Be(restaurant.Id);
        command.Name.Should().Be(restaurant.Name);
        command.Description.Should().Be(restaurant.Description);
        command.Category.Should().Be(restaurant.Category);
        command.HasDelivery.Should().Be(restaurant.HasDelivery);
    }
}