using System;
using CertificateRepository.ComponentModel;
using CertificateRepository.Validators.Core;
using Moq;
using NUnit.Framework;

namespace CertificateRepository.UnitTests.Validators.Core {
    [TestFixture]
    public class AndValidatorTests {
        private const string Thumbprint = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";

        [Test]
        public void Should_Throw_Exception_When_Null_LHS() {
            // Arrange
            var rhs = new Mock<IValidator<string>>();

            // Act
            TestDelegate sit = () => new AndValidator<string>(null, rhs.Object);

            // Assert
            Assert.Throws<ArgumentNullException>(sit);
        }

        [Test]
        public void Should_Throw_Exception_When_Null_RHS() {
            // Arrange
            var lhs = new Mock<IValidator<string>>();

            // Act
            TestDelegate sit = () => new AndValidator<string>(lhs.Object, null);

            // Assert
            Assert.Throws<ArgumentNullException>(sit);
        }

        [Test]
        public void Should_Call_IsValid_On_Both_Validators() {
            // Arrange
            var rhs = new Mock<IValidator<string>>();
            rhs.Setup(x => x.IsValid(It.IsAny<string>())).Returns(new ValidationResult {Valid = true});
            var lhs = new Mock<IValidator<string>>();
            lhs.Setup(x => x.IsValid(It.IsAny<string>())).Returns(new ValidationResult {Valid = true});

            // Act
            var compound = new AndValidator<string>(lhs.Object, rhs.Object);
            compound.IsValid(Thumbprint);

            // Assert
            rhs.Verify(x => x.IsValid(It.IsAny<string>()));
            lhs.Verify(x => x.IsValid(It.IsAny<string>()));
        }
    }
}