namespace Karmr.Domain.Entities
{
    using Karmr.Common.Contracts;
    using Karmr.Domain.Events;
    using Karmr.Common.Helpers;
    using Karmr.Common.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    internal abstract class Entity
    {
        private const BindingFlags BindingAttr = BindingFlags.NonPublic
            | BindingFlags.Instance
            | BindingFlags.DeclaredOnly;

        private readonly IList<Event> events = new List<Event>();

        private int uncommittedEventCount;

        protected readonly IClock Clock;

        internal IReadOnlyList<IEvent> Events
        {
            get
            {
                return this.events.ToList().AsReadOnly();
            }
        }

        protected Entity(IClock clock, IEnumerable<IEvent> events)
        {
            this.Clock = clock;

            foreach (var @event in events)
            {
                this.Apply(@event);
                this.events.Add(@event as Event);
            }
            this.uncommittedEventCount = 0;
        }

        internal void Handle(ICommand command)
        {
            var handleMethod = this.GetMethodBySignature(new[] { command.GetType() });
            if (handleMethod == null)
            {
                throw new UnhandledCommandException(string.Format("Handle for command {0} not found on entity {1}", command.GetType(), this.GetType()));
            }

            // invoke wrapps any exception in TargetInvocationException,
            // to simplify debugging let's throw inner exception instead
            try
            {
                handleMethod.Invoke(this, new object[] { command });
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException ?? ex;
            }
        }

        internal IReadOnlyList<Event> GetUncommittedEvents()
        {
            return this.events.Skip(this.events.Count - this.uncommittedEventCount).ToList().AsReadOnly();
        }

        protected void Raise(Event @event)
        {
            this.Apply(@event);
            this.events.Add(@event);
            this.uncommittedEventCount++;
        }

        private void Apply(IEvent @event)
        {
            var applyMethod = this.GetMethodBySignature(new[] { @event.GetType() });
            if (applyMethod == null)
            {
                throw new UnhandledEventException(string.Format("Apply for event {0} not found on entity {1}", @event.GetType(), this.GetType()));
            }

            // invoke wrapps any exception in TargetInvocationException,
            // to simplify debugging let's throw inner exception instead
            try
            {
                applyMethod.Invoke(this, new object[] { @event });
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException ?? ex;
            }
        }

        private MethodInfo GetMethodBySignature(Type[] argumentTypes)
        {
            return this.GetType().GetMethodBySignature(typeof(void), argumentTypes, BindingAttr);
        }
    }
}