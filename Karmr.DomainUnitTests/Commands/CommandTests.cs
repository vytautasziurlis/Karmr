namespace Karmr.DomainUnitTests.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Karmr.Contracts.Commands;
    using Karmr.Domain.Commands;
    using Karmr.Domain.Entities;
    using Karmr.DomainUnitTests.Helpers;

    using NUnit.Framework;

    [TestFixture]
    public class CommandTests
    {
        [Test]
        public void AllCommandDescendantsAreImmutable()
        {
            Assembly.GetAssembly(typeof(Command)).GetTypes().Where(t => t.IsSubclassOf(typeof(Command))).ToList()
                .ForEach(TypeAssert.IsImmutable);
        }

        [Test]
        public void AllCommandDescendantsAreHandledByOneEntity()
        {
            var allAggregateTypes = Assembly.GetAssembly(typeof(Aggregate)).GetTypes()
                .Where(t => t.IsSubclassOf(typeof(Aggregate))).ToList();

            Assembly.GetAssembly(typeof(Command)).GetTypes()
                .Where(t => t.IsSubclassOf(typeof(Command))).ToList()
                .ForEach(x => this.AssertCommandIsHandledByOnlyOneEntity(x, allAggregateTypes));
        }

        private void AssertCommandIsHandledByOnlyOneEntity(Type commandType, List<Type> aggregateType)
        {
            var exceptionCount = aggregateType.Count(x => this.AggregateThrowsWhenHandlingCommand(commandType, x));

            Assert.True(aggregateType.Count > exceptionCount, "Command {0} is not handled by any aggregate", commandType);
            Assert.AreEqual(aggregateType.Count - 1, exceptionCount, "Command {0} is handled by more than one aggregate", commandType);
        }

        private bool AggregateThrowsWhenHandlingCommand(Type commandType, Type aggregateType)
        {
            try
            {
                var entity = Activator.CreateInstance(aggregateType) as Aggregate;
                entity.Handle(Activator.CreateInstance(commandType) as ICommand);
                return false;
            }
            catch(UnhandledCommandException)
            {
                return true;
            }
        }
    }
}