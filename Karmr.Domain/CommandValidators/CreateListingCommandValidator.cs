using FluentValidation;
using Karmr.Domain.Commands;

namespace Karmr.Domain.CommandValidators
{
    public class CreateListingCommandValidator : AbstractValidator<CreateListingCommand>
    {
        public CreateListingCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty().Length(1, 255);
            RuleFor(c => c.Description).NotEmpty().Length(1, 4000);
            RuleFor(c => c.LocationName).Length(0, 1000);
        }
    }
}