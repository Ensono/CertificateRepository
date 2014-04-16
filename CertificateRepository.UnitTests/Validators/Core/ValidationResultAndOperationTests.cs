using System.Collections.Generic;
using System.Linq;
using CertificateRepository.Validators.Core;
using NUnit.Framework;
using Should;

namespace CertificateRepository.UnitTests.Validators.Core {
    [TestFixture]
    public class ValidationResultAndOperationTests {
        [Test]
        public void Should_Peform_And_Operation_On_Two_Valid_Results() {
            // Arrange
            var rhs = new ValidationResult {ErrorMessages = new List<ValidationMessage>(), Valid = true};
            var lhs = new ValidationResult {ErrorMessages = new List<ValidationMessage>(), Valid = true};

            // Act
            var compound = rhs & lhs;

            // Assert
            compound.ErrorMessages.ShouldNotBeNull();
            compound.ErrorMessages.ShouldBeEmpty();
            compound.Valid.ShouldBeTrue();
        }

        [Test]
        public void Should_Perform_And_Operation_On_One_Valid_And_One_Valid_Result_Variant1() {
            // Arrange
            var rhs = new ValidationResult {ErrorMessages = new List<ValidationMessage>(), Valid = true};
            var lhs = new ValidationResult {
                ErrorMessages = new List<ValidationMessage> {
                    new ValidationMessage {Message = "Message", ParamName = "ParamName"}
                },
                Valid = false
            };

            // Act
            var compound = rhs & lhs;

            // Assert
            compound.ErrorMessages.ShouldNotBeNull();
            compound.ErrorMessages.Count.ShouldEqual(1);
            compound.ErrorMessages.First().Message.ShouldEqual("Message");
            compound.ErrorMessages.First().ParamName.ShouldEqual("ParamName");
            compound.Valid.ShouldBeFalse();
        }

        [Test]
        public void Should_Perform_And_Operation_On_One_Valid_And_One_Valid_Result_Variant2() {
            // Arrange
            var rhs = new ValidationResult {
                ErrorMessages = new List<ValidationMessage> {
                    new ValidationMessage {Message = "Message", ParamName = "ParamName"}
                },
                Valid = false
            };
            var lhs = new ValidationResult {ErrorMessages = new List<ValidationMessage>(), Valid = true};

            // Act
            var compound = rhs & lhs;

            // Assert
            compound.ErrorMessages.ShouldNotBeNull();
            compound.ErrorMessages.Count.ShouldEqual(1);
            compound.ErrorMessages.First().Message.ShouldEqual("Message");
            compound.ErrorMessages.First().ParamName.ShouldEqual("ParamName");
            compound.Valid.ShouldBeFalse();
        }

        [Test]
        public void Should_Perform_And_Operation_On_Two_Invalid_Results() {
            // Arrange
            var rhs = new ValidationResult {
                ErrorMessages = new List<ValidationMessage> {
                    new ValidationMessage {Message = "Message1", ParamName = "ParamName1"}
                },
                Valid = false
            };
            var lhs = new ValidationResult {
                ErrorMessages = new List<ValidationMessage> {
                    new ValidationMessage {Message = "Message2", ParamName = "ParamName2"}
                },
                Valid = false
            };

            // Act
            var compound = rhs & lhs;

            // Assert
            compound.ErrorMessages.ShouldNotBeNull();
            compound.ErrorMessages.Count.ShouldEqual(2);
            compound.ErrorMessages.First().Message.ShouldEqual("Message1");
            compound.ErrorMessages.First().ParamName.ShouldEqual("ParamName1");
            compound.ErrorMessages.Last().Message.ShouldEqual("Message2");
            compound.ErrorMessages.Last().ParamName.ShouldEqual("ParamName2");
            compound.Valid.ShouldBeFalse();
        }
    }
}