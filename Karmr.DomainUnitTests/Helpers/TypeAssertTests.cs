using NUnit.Framework;

namespace Karmr.DomainUnitTests.Helpers
{
    [TestFixture]
    public class TypeAssertTests
    {
        [Test]
        public void TypeAssertIsMutableTest()
        {
            TypeAssert.IsMutable(typeof(MutableObject));
        }

        [Test]
        public void TypeAssertIsImmutableTest()
        {
            TypeAssert.IsImmutable(typeof(ImmutableObject));
        }

        private class MutableObject
        {
            public bool Foo { get; set; }
        }

        private class ImmutableObject
        {
            public bool Bar { get; private set; }
        }
    }
}