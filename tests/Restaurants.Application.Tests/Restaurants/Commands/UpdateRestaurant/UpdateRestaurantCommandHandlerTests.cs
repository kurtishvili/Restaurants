using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Application.Tests.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandHandlerTests
{
    private readonly Mock<ILogger<UpdateRestaurantCommandHandler>> _loggerMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IRestaurantsRepository> _restaurantsRepositoryMock;
    private readonly Mock<IRestaurantAuthorizationService> _restaurantAuthorizetionserviceMock;

    private readonly UpdateRestaurantCommandHandler _handler;

    public UpdateRestaurantCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<UpdateRestaurantCommandHandler>>();
        _mapperMock = new Mock<IMapper>();
        _restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
        _restaurantAuthorizetionserviceMock = new Mock<IRestaurantAuthorizationService>();

        _handler = new UpdateRestaurantCommandHandler(
            _loggerMock.Object,
            _mapperMock.Object,
            _restaurantsRepositoryMock.Object,
            _restaurantAuthorizetionserviceMock.Object);
    }

    [Fact()]
    public async Task Handle_WithValidRequest_ShouldUpdateRestaurant()
    {
        // arrange 

        var restaurantId = 1;

        var command = new UpdateRestaurantCommand()
        {
            Id = restaurantId,
            Name = "New Test",
            Category = "New Description",
            HasDelivery = true
        };

        var restaurant = new Restaurant()
        {
            Id = restaurantId,
            Name = "Test",
            Description = "Test",
        };

        _restaurantsRepositoryMock.Setup(r => r.GetByIdAsync(restaurantId, CancellationToken.None)).ReturnsAsync(restaurant);

        _restaurantAuthorizetionserviceMock.Setup(r => r.Authorize(restaurant, ResourceOperation.Update)).Returns(true);

        // act

        await _handler.Handle(command, CancellationToken.None);

        // assert
        _restaurantsRepositoryMock.Verify(r => r.SaveChanges(), Times.Once);
        _mapperMock.Verify(m => m.Map(command, restaurant), Times.Once);
    }

    [Fact()]
    public async Task Handle_WithNonExistingRestaurant_ShouldThrowNotFoundException()
    {
        // arrange 

        var restaurantId = 2;

        var command = new UpdateRestaurantCommand()
        {
            Id = restaurantId
        };

        _restaurantsRepositoryMock.Setup(r => r.GetByIdAsync(restaurantId, CancellationToken.None)).ReturnsAsync((Restaurant?)null);

        // act

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // assert

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Restaurant with id: {restaurantId} doesn't exist");
    }

    [Fact()]
    public async Task Handle_WithUnauthorizedUser_ShouldThrowforbidException()
    {
        // arrange 

        var restaurantId = 3;

        var command = new UpdateRestaurantCommand()
        {
            Id = restaurantId
        };

        var restaurant = new Restaurant()
        {
            Id = restaurantId
        };

        _restaurantsRepositoryMock.Setup(r => r.GetByIdAsync(restaurantId, CancellationToken.None)).ReturnsAsync(restaurant);

        _restaurantAuthorizetionserviceMock.Setup(r => r.Authorize(restaurant, ResourceOperation.Update)).Returns(false);

        // act

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // assert

        await act.Should().ThrowAsync<ForbidException>();
    }
}
