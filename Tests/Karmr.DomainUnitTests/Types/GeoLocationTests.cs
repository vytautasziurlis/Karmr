using System;
using Karmr.Common.Types;
using NUnit.Framework;

namespace Karmr.DomainUnitTests.Types
{
    public class GeoLocationTests
    {
        [TestCase(-91)]
        [TestCase(-90.1)]
        [TestCase(-90.00000001)]
        [TestCase(90.00000001)]
        [TestCase(90.1)]
        [TestCase(91)]
        public void ConstructorThrowsWhenLatitudeInvalid(decimal latitude)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new GeoLocation(latitude, 0));
        }

        [TestCase(-181)]
        [TestCase(-180.1)]
        [TestCase(-180.00000001)]
        [TestCase(180.00000001)]
        [TestCase(180.1)]
        [TestCase(181)]
        public void ConstructorThrowsWhenLongitudeInvalid(decimal longitude)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new GeoLocation(0, longitude));
        }

        [Test]
        public void EqualityOperatorChecksProperties()
        {
            var location1 = new GeoLocation(0.01m, 45.123m);
            var location2 = new GeoLocation(0.01m, 45.123m);

            Assert.True(location1 == location2);
        }
    }
}