using System.Linq;
using CertificateRepository.Validators;
using NUnit.Framework;
using Should;

namespace CertificateRepository.UnitTests.Validators {
    [TestFixture]
    public class ThumbprintCharacterValidatorTest {
        private static char[] ValidCharacters() {
            return "0123456789AaBbCcDdEeFf".ToCharArray();
        }

        private static char[] InvalidCharacters() {
            return " GgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz¬`!\"£$%^&*()_-+=[{]}:;@'~#|\\<,>.?/".ToCharArray();
        }

        [Test, TestCaseSource("ValidCharacters")]
        public void Should_Validate_Valid_Character(char input) {
            // Arrange
            var validator = new ThumbprintCharacterValidator();

            // Act
            var result = validator.IsValid(input);

            // Assert
            result.ShouldNotBeNull();
            result.Valid.ShouldBeTrue();
            result.ErrorMessages.ShouldNotBeNull();
            result.ErrorMessages.ShouldBeEmpty();
        }

        [Test, TestCaseSource("InvalidCharacters")]
        public void Should_Identify_Invalid_Character_In_Input(char input) {
            // Arrange
            var validator = new ThumbprintCharacterValidator();

            // Act
            var result = validator.IsValid(input);
            
            // Assert
            result.ShouldNotBeNull();
            result.Valid.ShouldBeFalse();
            result.ErrorMessages.ShouldNotBeNull();
            result.ErrorMessages.ShouldNotBeEmpty();
            result.ErrorMessages.Count.ShouldEqual(1);
            result.ErrorMessages.Single().ParamName.ShouldEqual("input");
            result.ErrorMessages.Single().Message.ShouldEqual(string.Format("The character '\\u{0:X4}' is not valid in this context.", (int)input));
        }
    }
}
