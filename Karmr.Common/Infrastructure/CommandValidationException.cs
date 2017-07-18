namespace Karmr.Common.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentValidation.Results;

    public class CommandValidationException : Exception
    {
        public CommandValidationException(IEnumerable<ValidationFailure> validationErrors)
            : base("CommandValidator raised validation errors: " + string.Join(Environment.NewLine, validationErrors.Select(x => x.ErrorMessage)))
        {
        }
    }
}