using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace Karmr.Domain.Infrastructure
{
    public class CommandValidationException : Exception
    {
        private IEnumerable<ValidationFailure> validationErrors;

        public CommandValidationException(IEnumerable<ValidationFailure> validationErrors)
            : base("CommandValidator raised validation errors: " + string.Join(Environment.NewLine, validationErrors.Select(x => x.ErrorMessage)))
        {
            this.validationErrors = validationErrors;
        }
    }
}