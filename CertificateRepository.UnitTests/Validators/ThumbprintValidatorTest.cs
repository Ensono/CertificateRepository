using System;
using CertificateRepository.ComponentModel;
using CertificateRepository.Validators;
using CertificateRepository.Validators.Core;
using Moq;
using NUnit.Framework;
using Should;

namespace CertificateRepository.UnitTests.Validators {
    [TestFixture]
    public class ThumbprintValidatorTest {
        private Mock<IValidator<string>> _beginsWithValidatorRtl;
        private Mock<IValidator<string>> _endsWithValidatorRtl;
        private Mock<IValidator<string>> _stringValidator;

        [SetUp]
        public void SetUp() {
            // Arrange
            _beginsWithValidatorRtl = new Mock<IValidator<string>>();
            _endsWithValidatorRtl = new Mock<IValidator<string>>();
            _stringValidator = new Mock<IValidator<string>>();
        }

        [Test]
        public void Should_Throw_ArgumentNullException_If_BeginsWithLeftToRight_Null() {
            // Act
            TestDelegate sit = () => new ThumbprintValidator(null, _beginsWithValidatorRtl.Object, _stringValidator.Object);

            // Assert
            var ex = Assert.Throws<ArgumentNullException>(sit);
            ex.ParamName.ShouldEqual("beginsWithLeftToRightCharacterValdiator");
            ex.Message.ShouldEqual("Value cannot be null." + Environment.NewLine + "Parameter name: beginsWithLeftToRightCharacterValdiator");
        }

        [Test]
        public void Should_Throw_ArgumentNullException_If_EndWithLeftToRight_Null() {
            // Act
            TestDelegate sit = () => new ThumbprintValidator(_endsWithValidatorRtl.Object, null, _stringValidator.Object);

            // Assert
            var ex = Assert.Throws<ArgumentNullException>(sit);
            ex.ParamName.ShouldEqual("endsWithLeftToRightCharacterValdiator");
            ex.Message.ShouldEqual("Value cannot be null." + Environment.NewLine + "Parameter name: endsWithLeftToRightCharacterValdiator");
        }

        [Test]
        public void Should_Throw_ArgumentNullException_If_ThumbprintStringVaidator_Null() {
            // Act
            TestDelegate sit = () => new ThumbprintValidator(_endsWithValidatorRtl.Object, _beginsWithValidatorRtl.Object, null);

            // Assert
            var ex = Assert.Throws<ArgumentNullException>(sit);
            ex.ParamName.ShouldEqual("stringValidator");
            ex.Message.ShouldEqual("Value cannot be null." + Environment.NewLine + "Parameter name: stringValidator");
        }

        [Test]
        public void Should_Call_IsValid_On_Subordinate_Validators() {
            // Arrange
            const string thumbprint = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";
            _beginsWithValidatorRtl.Setup(x => x.IsValid(It.IsAny<string>())).Returns(ValidationResult.Success);
            _endsWithValidatorRtl.Setup(x => x.IsValid(It.IsAny<string>())).Returns(ValidationResult.Success);
            _stringValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(ValidationResult.Success);
            var validator = new ThumbprintValidator(_beginsWithValidatorRtl.Object, _endsWithValidatorRtl.Object, _stringValidator.Object);

            // Act
            validator.IsValid(thumbprint);

            // Assert
            _beginsWithValidatorRtl.Verify(x => x.IsValid(thumbprint), Times.Once, "beginsWithLeftToRightCharacterValdiator not called.");
            _endsWithValidatorRtl.Verify(x => x.IsValid(thumbprint), Times.Once, "endsWithLeftToRightCharacterValdiator not called.");
            _stringValidator.Verify(x => x.IsValid(thumbprint), Times.Once, "stringValidator not called.");
        }
    }
}