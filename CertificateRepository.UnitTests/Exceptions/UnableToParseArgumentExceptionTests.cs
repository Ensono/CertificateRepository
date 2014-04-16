using System;
using System.Diagnostics;
using CertificateRepository.Exceptions;
using NUnit.Framework;
using Should;

namespace CertificateRepository.UnitTests.Exceptions {

    [TestFixture]
    public class UnableToParseArgumentExceptionTests {
        private UnableToParseArgumentException CreateException() {
            return new UnableToParseArgumentException("param", "invalid", typeof (TestEnumeration));
        }

        [Test]
        public void Should_Not_Throw_Exception_When_Instantiated() {
            Assert.DoesNotThrow(() => CreateException());
        }

        [Test]
        public void Should_Set_Parameter_Property() {
            var ex = CreateException();

            ex.ParamName.ShouldEqual("param");
        }

        [Test]
        public void Should_Set_Message_Property() {
            var ex = CreateException();

            ex.Message.ShouldEqual("Could not parse 'invalid' as Type TestEnumeration for the 'param' parameter." + Environment.NewLine + "Parameter name: param");
        }

        [Test]
        public void Should_Set_InnerException_Property() {
            var ex = CreateException();

            Debug.WriteLine(ex.InnerException);

            ex.InnerException.ShouldNotBeNull();
            ex.InnerException.Message.ShouldContain("Could not parse 'invalid' as Type TestEnumeration");
            ex.InnerException.Message.ShouldContain("it must be one of the following values:");
            ex.InnerException.Message.ShouldContain(" - ValidValue");
            ex.InnerException.Message.ShouldContain(" - AnotherValidValue");
        }

        private enum TestEnumeration {
            // ReSharper disable UnusedMember.Local
            ValidValue,
            AnotherValidValue
            // ReSharper restore UnusedMember.Local
        }
    }
}