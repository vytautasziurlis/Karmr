namespace Karmr.DomainUnitTests.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Karmr.Domain.Commands;
    using Karmr.Domain.Entities;
    using Karmr.DomainUnitTests.Helpers;

    using NUnit.Framework;
    using Domain.Helpers;

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
        public void AllCommandDescendantsHaveExactlyOneConstructor()
        {
            Assembly.GetAssembly(typeof(Command)).GetTypes().Where(t => t.IsSubclassOf(typeof(Command))).ToList()
                .ForEach(this.AssertCommandHasExactlyOneConstructor);
        }

        [Test]
        public void AllCommandDescendantsAreHandledByOneEntity()
        {
            var allEntityTypes = Assembly.GetAssembly(typeof(Entity)).GetTypes()
                .Where(t => t.IsSubclassOf(typeof(Entity))).ToList();

            Assembly.GetAssembly(typeof(Command)).GetTypes()
                .Where(t => t.IsSubclassOf(typeof(Command))).ToList()
                .ForEach(x => this.AssertCommandIsHandledByOnlyOneEntity(x, allEntityTypes));
        }

        private void AssertCommandHasExactlyOneConstructor(Type commandType)
        {
            var bindingFlags = BindingFlags.Public
                | BindingFlags.Static
                | BindingFlags.NonPublic
                | BindingFlags.Instance;

            var constructorCount = commandType.GetConstructors(bindingFlags).Length;
            Assert.AreEqual(1, constructorCount, "Command {0} has {1} constructors, expected 1", commandType, constructorCount);
        }

        private void AssertCommandIsHandledByOnlyOneEntity(Type commandType, List<Type> entityTypes)
        {
            var bindingFlags = BindingFlags.NonPublic
                | BindingFlags.Instance
                | BindingFlags.DeclaredOnly;

        var handlers = entityTypes.Count(x => x.GetMethodBySignature(typeof(void), new[] { commandType }, bindingFlags) != null);

            Assert.True(handlers > 0, "Command {0} is not handled by any entity", commandType);
            Assert.AreEqual(1, handlers, "Command {0} is handled by more than one entity", commandType);
        }
    }
}