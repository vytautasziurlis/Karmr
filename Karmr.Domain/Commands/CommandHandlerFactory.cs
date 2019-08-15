using System;
using Karmr.Common.Contracts;
using Karmr.Domain.Denormalizers;
using Karmr.Domain.Entities;

namespace Karmr.Domain.Commands
{
    public static class CommandHandlerFactory
    {
        private static readonly Type[] DenormalizeTypes =
        {
            typeof(ListingDenormalizer),
            typeof(ListingDiscussionDenormalizer),
            typeof(ListingOfferDenormalizer)
        };

        private static readonly Type[] EntityTypes = { typeof(Listing) };

        public static ICommandHandler Create(IClock clock, IEventRepository eventRepository, IDenormalizerRepository denormalizerRepository)
        {
            var denormalizerHandler = new DenormalizerHandler(denormalizerRepository, DenormalizeTypes);

            return new CommandHandler(clock, eventRepository, EntityTypes, denormalizerHandler);
        }
    }
}