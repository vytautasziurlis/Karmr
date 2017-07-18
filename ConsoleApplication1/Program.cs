namespace ConsoleApplication1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Karmr.Common.Contracts;
    using Karmr.Domain.Commands;
    using Karmr.Domain.Events;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    class Program
    {
        static void Main(string[] args)
        {
            //var command = new CreateListingCommand(Guid.NewGuid(), "description");
            //var typeName = command.GetType().AssemblyQualifiedName;
            

            //string serialized = JsonConvert.SerializeObject(command);

            //Console.WriteLine(serialized);

            //var type = Type.GetType(typeName);

            //ICommand deserialized = (ICommand)JsonConvert.DeserializeObject(serialized, type);

            //Console.ReadKey();

            //var @event = new ListingCreated(Guid.NewGuid(), Guid.NewGuid(), "description", DateTime.UtcNow);

            //var eventTypeName = @event.GetType().AssemblyQualifiedName;

            //var settings = new JsonSerializerSettings { ContractResolver = new MyContractResolver() };
            //var serializedEvent = JsonConvert.SerializeObject(@event, settings);
            //Console.WriteLine(serializedEvent);
            //var eventType = Type.GetType(eventTypeName);

            


            //IEvent deserializedEvent = (IEvent)JsonConvert.DeserializeObject(serializedEvent, eventType, settings);
            //Console.ReadKey();
        }

        public class MyContractResolver : Newtonsoft.Json.Serialization.DefaultContractResolver
        {
            protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
            {
                var props = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Select(p => base.CreateProperty(p, memberSerialization))
                    .ToList();
                props.ForEach(p => { p.Writable = true; p.Readable = true; });
                return props;
            }
        }
    }
}