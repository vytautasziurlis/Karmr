using FluentValidation;
using Karmr.Domain.Commands;

namespace Karmr.Domain.CommandValidators
{
    public class CreateListingDiscussionThreadCommandValidator : AbstractValidator<CreateListingDiscussionThreadCommand>
    {
        public CreateListingDiscussionThreadCommandValidator()
        {
            RuleFor(c => c.Content).NotEmpty().Length(1, 4000);
        }
    }
}