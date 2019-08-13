using FluentValidation;
using Karmr.Domain.Commands;

namespace Karmr.Domain.CommandValidators
{
    public class CreateListingDiscussionPostCommandValidator : AbstractValidator<CreateListingDiscussionPostCommand>
    {
        public CreateListingDiscussionPostCommandValidator()
        {
            RuleFor(c => c.Content).NotEmpty().Length(1, 4000);
        }
    }
}