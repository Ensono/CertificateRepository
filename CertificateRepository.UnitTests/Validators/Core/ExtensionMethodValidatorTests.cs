using CertificateRepository.ComponentModel;
using CertificateRepository.Validators.Core;
using Moq;
using NUnit.Framework;
using Should;

namespace CertificateRepository.UnitTests.Validators.Core {
    [TestFixture]
    public class ExtensionMethodValidatorTests {
        [Test]
        public void Should_Return_And_Validator() {
            // Arrange
            var validator1 = new Mock<IValidator<string>>();
            var validator2 = new Mock<IValidator<string>>();

            // Act
            var combined = validator1.Object.And(validator2.Object);

            // Assert
            combined.ShouldBeType<AndValidator<string>>();
        }

        [Test]
        public void Should_Return_Or_Validator() {
            // Arrange
            var validator1 = new Mock<IValidator<string>>();
            var validator2 = new Mock<IValidator<string>>();

            // Act
            var combined = validator1.Object.Or(validator2.Object);

            // Assert
            combined.ShouldBeType<OrValidator<string>>();
        }

        [Test]
        public void Should_Return_Not_Validator() {
            // Arrange
            var validator = new Mock<IValidator<string>>();

            // Act
            var combined = validator.Object.Not();

            // Assert
            combined.ShouldBeType<NotValidator<string>>();
        }
    }
}
