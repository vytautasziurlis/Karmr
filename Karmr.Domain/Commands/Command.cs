using System;
using Karmr.Contracts;
using System.Reflection;
using System.Linq;
using FluentValidation;
using Karmr.Domain.Infrastructure;

namespace Karmr.Domain.Commands
{
    public abstract class Command : ICommand
    {
        public Guid EntityKey { get; private set; }

        public Guid UserId { get; private set; }

        public Command(Guid userId)
        {
            this.EntityKey = Guid.NewGuid();
            this.UserId = userId;
        }

        public override string ToString()
        {
            return string.Format("{0}: ", nameof(this.GetType));
        }

        public void Validate()
        {
            var validatorType = typeof(AbstractValidator<>).MakeGenericType(this.GetType());

            var validationResults = Assembly.GetAssembly(this.GetType()).GetTypes()
                .Where(x => x.IsSubclassOf(validatorType))
                .Select(x => Activator.CreateInstance(x) as IValidator)
                .Select(x => x.Validate(this))
                .ToList();

            if (validationResults.Any(x => !x.IsValid))
            {
                throw new CommandValidationException(validationResults.SelectMany(x => x.Errors));
            }
        }
    }
}