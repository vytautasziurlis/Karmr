using Karmr.Domain.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Karmr.DomainUnitTests.Helpers
{
    public class TypeExtensionTests
    {
        private const BindingFlags BindingAttr = BindingFlags.NonPublic
            | BindingFlags.Instance
            | BindingFlags.DeclaredOnly;

        [TestCase(typeof(InternalMethodsOnly), typeof(void), new Type[] { }, "NoParamsReturnsVoid")]
        [TestCase(typeof(InternalMethodsOnly), typeof(bool), new Type[] { typeof(object) }, "OneParamReturnsBool")]
        [TestCase(typeof(InternalMethodsOnly), typeof(object), new Type[] { typeof(object), typeof(int), typeof(List<int>) }, "ThreeParamsReturnsObject")]
        public void GetMethodBySignatureReturnsExpectedInternalMethod(Type objectType, Type returnType, Type[] parameterTypes, string methodName)
        {
            var method = objectType.GetMethodBySignature(returnType, parameterTypes, BindingAttr);
            Assert.NotNull(method);
            Assert.AreEqual(methodName, method.Name);
        }

        [TestCase(typeof(PublicMethodsOnly), typeof(void), new Type[] { })]
        [TestCase(typeof(PublicMethodsOnly), typeof(bool), new Type[] { typeof(object) })]
        [TestCase(typeof(PublicMethodsOnly), typeof(object), new Type[] { typeof(object), typeof(int), typeof(List<int>) })]
        public void GetMethodBySignatureReturnsNullWhenMethodsPublic(Type objectType, Type returnType, Type[] parameterTypes)
        {
            var method = objectType.GetMethodBySignature(returnType, parameterTypes, BindingAttr);
            Assert.Null(method);
        }

        private class PublicMethodsOnly
        {
            public void NoParamsReturnsVoid()
            {
            }

            public bool OneParamReturnsBool(object obj)
            {
                return true;
            }

            public object ThreeParamsReturnsObject(object obj, int i, List<int> list)
            {
                return new object();
            }
        }

        private class InternalMethodsOnly
        {
            internal void NoParamsReturnsVoid()
            {
            }

            internal bool OneParamReturnsBool(object obj)
            {
                return true;
            }

            internal object ThreeParamsReturnsObject(object obj, int i, List<int> list)
            {
                return new object();
            }
        }
    }
}