using System.Collections.Generic;
using CertificateRepository.Validators.Core;
using NUnit.Framework;
using Should;

namespace CertificateRepository.UnitTests.Validators.Core {
    [TestFixture]
    public class ValidationResultNotOperationTests {
        [Test]
        public void Should_Negate_True_Result() {
            // Arrange
            var start = new ValidationResult {ErrorMessages = new List<ValidationMessage>(), Valid = true};

            // Act
            var negated = !start;

            // Assert
            negated.ErrorMessages.ShouldNotBeNull();
            negated.ErrorMessages.ShouldBeEmpty();
            negated.Valid.ShouldBeFalse();
        }

        [Test]
        public void Should_Negate_False_Result() {
            // Arrange
            var start = new ValidationResult {
                ErrorMessages = new List<ValidationMessage>
                {
                    new ValidationMessage { Message = "Message", ParamName = "ParamName"}
                }, Valid = false
            };

            // Act
            var negated = !start;

            // Assert
            negated.ErrorMessages.ShouldNotBeNull();
            negated.ErrorMessages.Count.ShouldEqual(1);
            negated.Valid.ShouldBeTrue();
        }
    }
}
