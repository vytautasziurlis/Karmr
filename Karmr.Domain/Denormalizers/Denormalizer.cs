using Karmr.Common.Contracts;
using Karmr.Common.Helpers;
using Karmr.Common.Infrastructure;
using System;
using System.Reflection;

namespace Karmr.Domain.Denormalizers
{
    internal abstract class Denormalizer
    {
        private const BindingFlags BindingAttr = BindingFlags.NonPublic
            | BindingFlags.Instance
            | BindingFlags.DeclaredOnly;

        protected IDenormalizerRepository Repository;

        protected Denormalizer(IDenormalizerRepository repository)
        {
            this.Repository = repository;
        }

        internal void Apply(IEvent @event)
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