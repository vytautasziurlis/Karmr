using Karmr.Domain.Commands;
using System;

namespace Karmr.DomainUnitTests.Builders
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    internal class CommandBuilder<T> where T : Command
    {
        private readonly Dictionary<string, object> constructorArguments;

        internal CommandBuilder()
        {
            this.constructorArguments = CommandBuilderHelper.GetConstructorArguments(typeof(T));
        }

        internal CommandBuilder<T> With<TPropertyType>(Expression<Func<T, TPropertyType>> property, TPropertyType value)
        {
            var propName = ((MemberExpression)property.Body).Member.Name;
            if (!this.constructorArguments.ContainsKey(propName))
            {
                this.constructorArguments.Add(propName, null);
            }
            this.constructorArguments[propName] = value;

            return this;
        }

        internal T Build()
        {
            return (T)Activator.CreateInstance(typeof(T), this.constructorArguments.Select(x => x.Value).ToArray());
        }
    }
}