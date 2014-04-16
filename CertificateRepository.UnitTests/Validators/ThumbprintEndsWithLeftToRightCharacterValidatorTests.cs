using CertificateRepository.Validators;
using NUnit.Framework;
using Should;

namespace CertificateRepository.UnitTests.Validators {
    [TestFixture]
    public class ThumbprintEndsWithLeftToRightCharacterValidatorTests {
        private const string ValidThumpbrint = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";
        private const string InvalidThumpbrint = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA\u200E";

        [Test]
        public void Should_Not_End_With_LeftToRightCharacter() {
            ValidThumpbrint.IsNormalized().ShouldBeTrue();
            ValidThumpbrint.Length.ShouldEqual(40);
            ValidThumpbrint[39].ShouldNotEqual('\u200E');
        }

        [Test]
        public void Should_End_With_LeftToRightCharacter() {
            InvalidThumpbrint.IsNormalized().ShouldBeTrue();
            InvalidThumpbrint.Length.ShouldEqual(41);
            InvalidThumpbrint[40].ShouldEqual('\u200E');
        }

        [Test]
        public void Should_Mark_Normal_Thumbprint_As_Valid() {
            // Arrange
            var validator = new ThumbprintEndsWithLeftToRightCharacterValidator();

            // Act
            var result = validator.IsValid(ValidThumpbrint);

            // Assert
            result.Valid.ShouldBeTrue();
        }

        [Test]
        public void Should_Mark_Invalid_Thumbprint_As_Invalid() {
            // Arrange
            var validator = new ThumbprintEndsWithLeftToRightCharacterValidator();

            // Act
            var result = validator.IsValid(InvalidThumpbrint);

            // Assert
            result.Valid.ShouldBeFalse();
        }
    }
}