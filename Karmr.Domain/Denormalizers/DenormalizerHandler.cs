using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Karmr.Common.Contracts;
using Karmr.Common.Helpers;
using Karmr.Common.Infrastructure;

namespace Karmr.Domain.Denormalizers
{
    internal sealed class DenormalizerHandler : IDenormalizerHandler
    {
        private readonly BindingFlags BindingFlags = BindingFlags.NonPublic
                                                     | BindingFlags.Instance
                                                     | BindingFlags.DeclaredOnly;

        private readonly IDenormalizerRepository denormalizerRepository;

        private readonly IEnumerable<Type> denormalizerTypes;

        private readonly Dictionary<Type, Type> eventDenormalizers = new Dictionary<Type, Type>();

        public DenormalizerHandler(IDenormalizerRepository denormalizerRepository, IEnumerable<Type> denormalizerTypes)
        {
            this.denormalizerRepository = denormalizerRepository;
            this.denormalizerTypes = denormalizerTypes;
        }

        public void Handle(IEnumerable<IEvent> events)
        {
            foreach (var @event in events)
            {
                var denormalizer = this.GetDenormalizerInstance(@event.GetType());
                denormalizer.Apply(@event);
            }
        }

        private Denormalizer GetDenormalizerInstance(Type eventType)
        {
            var denormalizerType = this.GetDenormalizerType(eventType);
            var @params = new object[] { this.denormalizerRepository };

            return Activator.CreateInstance(denormalizerType, this.BindingFlags, null, @params, null) as Denormalizer;
        }

        private Type GetDenormalizerType(Type eventType)
        {
            if (!this.eventDenormalizers.ContainsKey(eventType))
            {
                var matchingDenormalizerTypes = this.denormalizerTypes
                    .Where(t => t.IsSubclassOf(typeof(Denormalizer))).ToList()
                    .Where(t => t.GetMethodBySignature(typeof(void), new[] { eventType }, this.BindingFlags) != null)
                    .ToList();

                if (matchingDenormalizerTypes.Count != 1)
                {
                    throw new UnhandledEventException(string.Format("Expected exactly one denormalizer to handle event {0}, found {1} instead.",
                        eventType,
                        matchingDenormalizerTypes.Count));
                }

                this.eventDenormalizers.Add(eventType, matchingDenormalizerTypes.First());
            }
            return this.eventDenormalizers[eventType];
        }
    }
}