using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CertificateRepository.Exceptions;
using CertificateRepository.Validators.Core;
using NUnit.Framework;
using Should;

namespace CertificateRepository.UnitTests.Exceptions {
    [TestFixture]
    public class CompositeExceptionTests {
        [Test]
        public void Should_Output_Two_Inner_Exception_Upon_ToString() {
            // Arrange
            var validationMessages = new List<ValidationMessage> {
                new ValidationMessage {Message = "Message1", ParamName = "ParamName1"},
                new ValidationMessage {Message = "Message2", ParamName = "ParamName2"}
            };

            // Act
            var ex = new CompositeArgumentException(validationMessages);

            // Assert
            ex.ShouldNotBeNull();
            var lines = ex.ToString().Split(new [] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            lines.Length.ShouldEqual(7);
            lines[0].ShouldEqual(
                    "CertificateRepository.Exceptions.CompositeArgumentException: Exception of type 'CertificateRepository.Exceptions.CompositeArgumentException' was thrown.");

            lines[1].ShouldEqual("  ------ Inner Exception #1 ------");
            lines[2].ShouldEqual("System.ArgumentException: Message1");
            lines[3].ShouldEqual("Parameter name: ParamName1");
            lines[4].ShouldEqual("  ------ Inner Exception #2 ------");
            lines[5].ShouldEqual("System.ArgumentException: Message2");
            lines[6].ShouldEqual("Parameter name: ParamName2");
        }
    }

    [TestFixture]
    public class CompositeArgumentExceptionTests {
        [Test]
        public void Should_Throw_Exception_If_ValidationMessages_Null() {
            // Arrange
            TestDelegate sit = () => new CompositeArgumentException(null);

            // Act
            var ex = Assert.Throws<ArgumentNullException>(sit);

            // Assert
            ex.ParamName.ShouldEqual("validationMessages");
        }

        [Test]
        public void Should_Throw_Exception_If_ValidationMessages_Empty() {
            // Arrange
            var validationMessages = new List<ValidationMessage>();
            TestDelegate sit = () => new CompositeArgumentException(validationMessages);

            // Act
            var ex = Assert.Throws<ArgumentException>(sit);

            // Assert
            ex.ParamName.ShouldEqual("validationMessages");
        }

        [Test]
        public void Should_Initialize_InnerExceptions_With_One_ValidationMessage() {
            // Arrange
            var validationMessages = new List<ValidationMessage> {
                new ValidationMessage {Message = "Message1", ParamName = "ParamName1"},
            };

            // Act
            var ex = new CompositeArgumentException(validationMessages);

            // Assert
            ex.ShouldNotBeNull();
            ex.InnerExceptions.ShouldNotBeNull();
            ex.InnerExceptions.ShouldNotBeEmpty();
            ex.InnerExceptions.First().Message.ShouldStartWith("Message1");
            ex.InnerExceptions.First().ParamName.ShouldEqual("ParamName1");
        }

        [Test]
        public void Should_Initialize_InnerExceptions_With_Two_ValidationMessages() {
            // Arrange
            var validationMessages = new List<ValidationMessage> {
                new ValidationMessage {Message = "Message1", ParamName = "ParamName1"},
                new ValidationMessage {Message = "Message2", ParamName = "ParamName2"}
            };

            // Act
            var ex = new CompositeArgumentException(validationMessages);

            // Assert
            ex.ShouldNotBeNull();
            ex.InnerExceptions.ShouldNotBeNull(); 
            ex.InnerExceptions.ShouldNotBeEmpty();
            ex.InnerExceptions.First().Message.ShouldStartWith("Message1");
            ex.InnerExceptions.First().ParamName.ShouldEqual("ParamName1");
            ex.InnerExceptions.Last().Message.ShouldStartWith("Message2");
            ex.InnerExceptions.Last().ParamName.ShouldEqual("ParamName2");
        }
    }
}
