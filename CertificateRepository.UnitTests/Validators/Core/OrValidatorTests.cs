using System;
using CertificateRepository.ComponentModel;
using CertificateRepository.Validators.Core;
using Moq;
using NUnit.Framework;
using Should;

namespace CertificateRepository.UnitTests.Validators.Core {
    [TestFixture]
    public class OrValidatorTests {
        private const string Thumbprint = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";

        [Test]
        public void Should_Throw_ArgumentNullException_When_LHS_Null() {
            // Arrange
            var rhs = new Mock<IValidator<string>>();
            rhs.Setup(x => x.IsValid(It.IsAny<string>())).Returns(ValidationResult.Success);

            // Act
            TestDelegate sit = () => new OrValidator<string>(null, rhs.Object);

            // Assert
            var ex = Assert.Throws<ArgumentNullException>(sit);
            ex.ParamName.ShouldEqual("validator1");
            ex.Message.ShouldEqual("Value cannot be null." + Environment.NewLine + "Parameter name: validator1");
        }

        [Test]
        public void Should_Throw_ArgumentNullException_When_RHS_Null() {
            // Arrange
            var lhs = new Mock<IValidator<string>>();
            lhs.Setup(x => x.IsValid(It.IsAny<string>())).Returns(ValidationResult.Success);

            // Act
            TestDelegate sit = () => new OrValidator<string>(lhs.Object, null);

            // Assert
            var ex = Assert.Throws<ArgumentNullException>(sit);
            ex.ParamName.ShouldEqual("validator2");
            ex.Message.ShouldEqual("Value cannot be null." + Environment.NewLine + "Parameter name: validator2");
        }

        [Test]
        public void Should_Call_IsValid_On_Both_Validators() {
            // Arrange
            var lhs = new Mock<IValidator<string>>();
            lhs.Setup(x => x.IsValid(It.IsAny<string>())).Returns(ValidationResult.Success);
            var rhs = new Mock<IValidator<string>>();
            rhs.Setup(x => x.IsValid(It.IsAny<string>())).Returns(ValidationResult.Success);
            var validator = new OrValidator<string>(lhs.Object, rhs.Object);

            // Act
            validator.IsValid(Thumbprint);

            // Assert
            rhs.Verify(x => x.IsValid(It.IsAny<string>()));
            lhs.Verify(x => x.IsValid(It.IsAny<string>()));
        }
    }
}
