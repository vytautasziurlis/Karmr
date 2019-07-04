using System;
using System.Collections.Generic;

namespace Karmr.DomainUnitTests.Builders
{
    using Karmr.Domain.Commands;

    internal static class CommandBuilderHelper
    {
        private static readonly Dictionary<Type, Dictionary<string, object>> CommandConstructorArguments = new Dictionary<Type, Dictionary<string, object>>();

        static CommandBuilderHelper()
        {
            CommandConstructorArguments.Add(typeof(CreateListingCommand), new Dictionary<string, object>
            {
                { "UserId", Guid.NewGuid()},
                { "Name", "Name" },
                { "Description", "Description"}
            });

            CommandConstructorArguments.Add(typeof(UpdateListingCommand), new Dictionary<string, object>
            {
                { "EntityKey", Guid.NewGuid() },
                { "UserId", Guid.NewGuid() },
                { "Name", "Name" },
                { "Description", "Description" }
            });
        }

        internal static Dictionary<string, object> GetConstructorArguments(Type type)
        {
            if (!CommandConstructorArguments.ContainsKey(type))
            {
                throw new KeyNotFoundException(string.Format("CommandBuilderHelper - default constructor parameters not found for type {0}", type.Name));
            }
            return new Dictionary<string, object>(CommandConstructorArguments[type]);
        }
    }
}