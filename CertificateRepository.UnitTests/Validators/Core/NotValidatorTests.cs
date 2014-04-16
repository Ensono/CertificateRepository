using System;
using CertificateRepository.ComponentModel;
using CertificateRepository.Validators.Core;
using Moq;
using NUnit.Framework;
using Should;

namespace CertificateRepository.UnitTests.Validators.Core {
    public class NotValidatorTests {
        private const string Thumbprint = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";

        [Test]
        public void Should_Throw_ArgumentNullException_If_Spec_Null() {
            TestDelegate sit = () => new NotValidator<string>(null);
            var ex = Assert.Throws<ArgumentNullException>(sit);
            ex.ParamName.ShouldEqual("validator");
            ex.Message.ShouldEqual("Value cannot be null." + Environment.NewLine + "Parameter name: validator");
        }

        [Test]
        public void Should_Call_IsValid() {
            // Arrange
            var validator = new Mock<IValidator<string>>();
            validator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(ValidationResult.Success);
            var notValidator = new NotValidator<string>(validator.Object);

            // Act
            notValidator.IsValid(Thumbprint);

            // Assert
            validator.Verify(x => x.IsValid(It.IsAny<string>()), Times.Once);
        }
    }
}
