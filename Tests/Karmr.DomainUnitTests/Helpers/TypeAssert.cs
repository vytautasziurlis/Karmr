using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Karmr.DomainUnitTests.Helpers
{
    public class TypeAssert
    {
        public static void IsMutable(Type type)
        {
            if (!GetMutableProperties(type).Any())
            {
                Assert.Fail("Type {0} has no public setters", type);
            }
        }

        public static void IsImmutable(Type type)
        {
            GetMutableProperties(type).ToList().ForEach(property =>
                Assert.Fail("Type {0} has public setter {1}", type, property.Name)
            );
        }

        private static IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            return type.GetTypeInfo().GetRuntimeProperties();
        }

        private static IEnumerable<PropertyInfo> GetMutableProperties(Type type)
        {
            return GetProperties(type).Where(PropertyIsMutable);
        }

        private static bool PropertyIsMutable(PropertyInfo property)
        {
            return property.GetSetMethod() != null;
        }
    }
}