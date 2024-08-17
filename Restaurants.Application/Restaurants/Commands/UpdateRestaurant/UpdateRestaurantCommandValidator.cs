using FluentValidation;

namespace Restaurants.Application.Restaurants.Commands.NewFolder
{
    internal class UpdateRestaurantCommandValidator : AbstractValidator<UpdateRestaurantCommand>
    {
        public UpdateRestaurantCommandValidator()
        {
            RuleFor(dto => dto.Name)
               .Length(3, 100);

            RuleFor(dto => dto.Description)
                .NotEmpty().WithMessage("Description is required");
        }
    }
}