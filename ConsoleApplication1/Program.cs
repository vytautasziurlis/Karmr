using System;
using System.Collections.Generic;

namespace ConsoleApplication1
{
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using Karmr.Contracts.Commands;
    using Karmr.Domain.Commands;

    using Newtonsoft.Json;

    class Program
    {
        static void Main(string[] args)
        {
            var command = new CreateListingCommand(Guid.NewGuid(), "description");
            var typeName = command.GetType().AssemblyQualifiedName;
            

            string serialized = JsonConvert.SerializeObject(command);

            Console.WriteLine(serialized);

            var type = Type.GetType(typeName);

            ICommand deserialized = (ICommand)JsonConvert.DeserializeObject(serialized, type);

            Console.ReadKey();
        }
    }
}