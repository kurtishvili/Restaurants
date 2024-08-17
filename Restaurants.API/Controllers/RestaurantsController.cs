using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
using Restaurants.Application.Restaurants.Commands.NewFolder;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurantById;
using Restaurants.Domain.Constants;

namespace Restaurants.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RestaurantsController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RestaurantDto>))]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var restaurants = await mediator.Send(new GetAllRestaurantsQuery(), cancellationToken);

            return Ok(restaurants);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var restaurant = await mediator.Send(new GetRestaurantByIdQuery(id), cancellationToken);

            return Ok(restaurant);
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Owner)]
        public async Task<IActionResult> CreateRestaurant(CreateRestaurantCommand command, CancellationToken cancellationToken)
        {
            var id = await mediator.Send(command);

            return CreatedAtAction(nameof(GetById), new { id }, null);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRestaurant([FromRoute] int id, CancellationToken cancellationToken)
        {
            await mediator.Send(new DeleteRestaurantCommand(id), cancellationToken);

            return NoContent();

        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateRestaurant([FromRoute] int id, UpdateRestaurantCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;

            await mediator.Send(command, cancellationToken);

            return NoContent();
        }
    }
}