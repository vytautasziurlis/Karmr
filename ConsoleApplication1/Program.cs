using System;
using System.Collections.Generic;
using System.Linq;
using Karmr.Domain.Commands;
using Karmr.Common.Contracts;
using Karmr.Persistence;
using Karmr.Domain.Entities;
using Karmr.Domain.Denormalizers;
using Karmr.Domain.Queries;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var clock = new SystemClock();
            var eventRepository = new SqlEventRepository("Server=.;Database=Karmr;User Id=Karmr;Password=Karmr;");
            var entityTypes = new List<Type> { typeof(Listing) };

            var denormalizerRepository = new DenormalizerRepository("Server=.;Database=Karmr;User Id=Karmr;Password=Karmr;");
            var denormalizerTypes = new List<Type> { typeof(ListingDenormalizer) };
            var denormalizerHandler = new DenormalizerHandler(denormalizerRepository, denormalizerTypes);

            var commandHandler = new CommandHandler(clock, eventRepository, entityTypes, denormalizerHandler);

            // handle some commands
            //var command1 = new CreateListingCommand(Guid.NewGuid(), "Listing 1", "Listing 1 description");
            //commandHandler.Handle(command1);

            //var command2 = new CreateListingCommand(Guid.NewGuid(), "Listing 2", "Listing 2 description");
            //commandHandler.Handle(command2);

            var queryRepo = new QueryRepository("Server=.;Database=Karmr;User Id=Karmr;Password=Karmr;");
            var listingQueries = new ListingQueries(queryRepo);
            var listings = listingQueries.GetAll();

            var firstListing = listingQueries.GetById(listings.First().Id);

            //var command3 = new UpdateListingCommand(
            //    firstListing.Id,
            //    new Guid("60B2859A-63EF-4B64-907B-E5E072D8CA9B"),
            //    firstListing.Name + " - updated",
            //    firstListing.Description + " - updated");
            //commandHandler.Handle(command3);

            foreach (var listing in listings)
            {
                Console.WriteLine($"{listing.Name} - {listing.Description}");
            }

            Console.WriteLine("Done.");
            Console.ReadKey();
        }
    }

    public class SystemClock : IClock
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}