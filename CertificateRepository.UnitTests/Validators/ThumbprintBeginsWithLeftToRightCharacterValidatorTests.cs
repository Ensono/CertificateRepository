using CertificateRepository.Validators;
using NUnit.Framework;
using Should;

namespace CertificateRepository.UnitTests.Validators {
    [TestFixture]
    public class ThumbprintBeginsWithLeftToRightCharacterValidatorTests {
        private const string ValidThumpbrint = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";
        private const string InvalidThumpbrint = "\u200EAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";

        [Test]
        public void Should_Not_StartWith_LeftToRightCharacter() {
            ValidThumpbrint.IsNormalized().ShouldBeTrue();
            ValidThumpbrint[0].ShouldNotEqual('\u200E');
        }

        [Test]
        public void Should_StartWith_LeftToRightCharacter() {
            InvalidThumpbrint.IsNormalized().ShouldBeTrue();
            InvalidThumpbrint[0].ShouldEqual('\u200E');
        }

        [Test]
        public void Should_Mark_Normal_Thumbprint_As_Valid() {
            // Arrange
            var validator = new ThumbprintBeginsWithLeftToRightCharacterValidator();

            // Act
            var result = validator.IsValid(ValidThumpbrint);

            // Assert
            result.Valid.ShouldBeTrue();
        }

        [Test]
        public void Should_Mark_Invalid_Thumbprint_As_Invalid() {
            // Arrange
            var validator = new ThumbprintBeginsWithLeftToRightCharacterValidator();

            // Act
            var result = validator.IsValid(InvalidThumpbrint);

            // Assert
            result.Valid.ShouldBeFalse();
        }
    }
}
