using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Karmr.Domain.Helpers
{
    internal static class TypeExtensions
    {
        public static MethodInfo GetMethodBySignature(this Type type, Type returnType, IEnumerable<Type> parameterTypes, BindingFlags bindingAttr)
        {
            return type
                .GetMethods(bindingAttr)
                .Where(x => x.ReturnType == returnType)
                .Where(x => MethodMathesParameterTypes(x, parameterTypes))
                .SingleOrDefault();
        }

        private static bool MethodMathesParameterTypes(MethodInfo methodInfo, IEnumerable<Type> parameterTypes)
        {
            return Enumerable.SequenceEqual(methodInfo.GetParameters().Select(x => x.ParameterType), parameterTypes);
        }
    }
}