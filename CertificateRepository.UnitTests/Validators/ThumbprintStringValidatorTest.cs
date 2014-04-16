using System;
using System.Linq;
using System.Text.RegularExpressions;
using CertificateRepository.ComponentModel;
using CertificateRepository.Validators;
using CertificateRepository.Validators.Core;
using Moq;
using NUnit.Framework;
using Should;

namespace CertificateRepository.UnitTests.Validators {
    [TestFixture]
    public class ThumbprintStringValidatorTest {
        private Mock<IValidator<char>> _characterValidator;
        private Mock<IValidator<string>> _lengthValidator;

        private ThumbprintStringValidator GetValidator() {
            return new ThumbprintStringValidator(_characterValidator.Object, _lengthValidator.Object);
        }

        [SetUp]
        public void SetUp() {
            _characterValidator = new Mock<IValidator<char>>();
            _lengthValidator = new Mock<IValidator<string>>();
        }

        [Test]
        public void Should_Throw_ArgumentNullException_If_CharacterValidator_Null() {
            // Arrange
            TestDelegate sit = () => new ThumbprintStringValidator(null, _lengthValidator.Object);

            // Act
            var ex = Assert.Throws<ArgumentNullException>(sit);

            // Assert
            ex.ParamName.ShouldEqual("thumbprintCharacterValidator");
        }

        [Test]
        public void Should_Throw_ArgumentNullException_If_LengthValidator_Null() {
            // Arrange
            TestDelegate sit = () => new ThumbprintStringValidator(_characterValidator.Object, null);

            // Act
            var ex = Assert.Throws<ArgumentNullException>(sit);

            // Assert
            ex.ParamName.ShouldEqual("lengthValidator");
        }

        [Test]
        public void Should_Call_CharacterValidator_Once() {
            // Arrange
            _characterValidator.Setup(x => x.IsValid(It.IsAny<char>())).Returns(ValidationResult.Success);
            _lengthValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(ValidationResult.Success);

            // Act
            GetValidator().IsValid("A");

            // Assert
            _characterValidator.Verify(x => x.IsValid(It.IsAny<char>()), Times.Once);
        }

        [Test]
        public void Should_Call_CharacterValidator_Forty_Times() {
            // Arrange
            _characterValidator.Setup(x => x.IsValid(It.IsAny<char>())).Returns(ValidationResult.Success);
            _lengthValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(ValidationResult.Success);

            // Act
            GetValidator().IsValid(new string('A', 40));

            // Assert
            _characterValidator.Verify(x => x.IsValid(It.IsAny<char>()), Times.Exactly(40));
        }

        [Test]
        public void Should_Return_Valid_When_All_Characters_Are_Valid() {
            // Arrange
            _characterValidator.Setup(x => x.IsValid(It.IsAny<char>())).Returns(ValidationResult.Success);
            _lengthValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(ValidationResult.Success);

            // Act
            var result = GetValidator().IsValid(new string('A', 40));

            // Assert
            result.ShouldNotBeNull();
            result.Valid.ShouldBeTrue();
            result.ErrorMessages.ShouldNotBeNull();
            result.ErrorMessages.ShouldBeEmpty();
        }

        [Test]
        public void Should_Return_Invalid_When_All_Characters_Are_Invalid() {
            // Arrange
            _characterValidator.Setup(x => x.IsValid(It.IsAny<char>())).Returns(ValidationResult.Failure("ABC", "thumbprint"));
            _lengthValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(ValidationResult.Success);
            var messageMatch = new Regex("Invalid character at position ([0-9]+): ABC", RegexOptions.Compiled);

            // Act
            var result = GetValidator().IsValid(new string('X', 40));

            // Assert
            result.ShouldNotBeNull();
            result.Valid.ShouldBeFalse();
            result.ErrorMessages.ShouldNotBeNull();
            result.ErrorMessages.Count.ShouldEqual(40);
            result.ErrorMessages.All(x => messageMatch.IsMatch(x.Message)).ShouldBeTrue();
            result.ErrorMessages.All(x => x.ParamName == "thumbprint").ShouldBeTrue();
        }

        [Test]
        public void Should_Call_Length_Validator_Once() {
            // Arrange
            _characterValidator.Setup(x => x.IsValid(It.IsAny<char>())).Returns(ValidationResult.Success);
            _lengthValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(ValidationResult.Success);

            // Act
            GetValidator().IsValid(new string('A', 40));

            // Assert
            _lengthValidator.Verify(x => x.IsValid(It.IsAny<string>()), Times.Once);
        }
    }
}