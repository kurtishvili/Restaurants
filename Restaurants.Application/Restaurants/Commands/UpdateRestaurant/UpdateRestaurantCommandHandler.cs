using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.NewFolder
{
    internal class UpdateRestaurantCommandHandler(ILogger<UpdateRestaurantCommandHandler> logger, IMapper mapper, IRestaurantsRepository restaurantsRepository) :
        IRequestHandler<UpdateRestaurantCommand>
    {
        public async Task Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Updating Restorant with id: {@RestaunratId} with {@UpdateRestaurant}", request.Id, request);

            var restaurant = await restaurantsRepository.GetByIdAsync(request.Id, cancellationToken);

            if (restaurant == null)
                throw new NotFoundException(nameof(Restaurant), request.Id.ToString());

            mapper.Map(request, restaurant);

            await restaurantsRepository.SaveChanges();
        }
    }
}
