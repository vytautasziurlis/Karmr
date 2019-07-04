using FluentValidation;
using Karmr.Domain.Commands;

namespace Karmr.Domain.CommandValidators
{
    public class UpdateListingCommandValidator : AbstractValidator<UpdateListingCommand>
    {
        public UpdateListingCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty().Length(1, 255);
            RuleFor(c => c.Description).NotEmpty().Length(1, 4000);
        }
    }
}