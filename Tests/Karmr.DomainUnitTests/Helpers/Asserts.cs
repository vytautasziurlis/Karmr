using System.ComponentModel;
using NUnit.Framework;

namespace Karmr.DomainUnitTests.Helpers
{
    internal static class Asserts
    {
        public static bool HaveSameProperties(object expected, object actual)
        {
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(expected))
            {
                var expectedValue = prop.GetValue(expected);

                var actualProperty = actual.GetType().GetProperty(prop.Name);
                if (actualProperty == null)
                {
                    throw new AssertionException($"Expected property '{prop.Name}' not found");
                }
                var actualValue = actualProperty.GetValue(actual);

                Assert.AreEqual(expectedValue, actualValue);
            }
            return true;
        }
    }
}