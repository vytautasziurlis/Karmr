using System;
using System.Collections.Generic;
using Karmr.Domain.Commands;
using Karmr.Common.Contracts;
using Karmr.Persistence;
using Karmr.Domain.Entities;
using Karmr.Domain.Denormalizers;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var clock = new SystemClock();
            var eventRepository = new SqlEventRepository("Server=.;Database=Karmr;User Id=Karmr;Password=Karmr;");
            var denormalizerRepository = new DenormalizerRepository("Server=.;Database=Karmr;User Id=Karmr;Password=Karmr;");
            var entityTypes = new List<Type> { typeof(Listing) };
            var denormalizerTypes = new List<Type> { typeof(ListingDenormalizer) };

            var commandHandler = new CommandHandler(clock, eventRepository, denormalizerRepository, entityTypes, denormalizerTypes);

            // handle some commands
            var command1 = new CreateListingCommand(Guid.NewGuid(), "Listing 1 description");
            //commandHandler.Handle(command1);

            var command2 = new CreateListingCommand(Guid.NewGuid(), "Listing 2 description");
            //commandHandler.Handle(command2);

            var command3 = new UpdateListingCommand(
                new Guid("0B45F610-FF87-4C8D-B860-9E68D15A77BA"),
                new Guid("87316104-CE1E-4C9D-8F1F-736B2F77086E"),
                "Listing 2 updated description");
            commandHandler.Handle(command3);

            Console.ReadKey();
        }
    }

    public class SystemClock : IClock
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}