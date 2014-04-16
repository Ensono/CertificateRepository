using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CertificateRepository.ComponentModel;
using CertificateRepository.Validators;
using CertificateRepository.Validators.Core;
using Moq;
using NUnit.Framework;
using Should;

namespace CertificateRepository.UnitTests.Validators {
    [TestFixture]
    public class StringLengthValidatorTests {
        [Test]
        public void Should_Return_Valid_When_String_Is_One_Character_Long() {
            // Arrange
            var validator = new StringLengthValidator(1);

            // Act
            var result = validator.IsValid(new string('A', 1));

            // Assert
            result.ShouldNotBeNull();
            result.Valid.ShouldBeTrue();
            result.ErrorMessages.ShouldNotBeNull();
            result.ErrorMessages.ShouldBeEmpty();
        }

        [Test]
        public void Should_Return_Valid_When_String_Is_Forty_Character_Long() {
            // Arrange
            var validator = new StringLengthValidator(40);

            // Act
            var result = validator.IsValid(new string('A', 40));

            // Assert
            result.ShouldNotBeNull();
            result.Valid.ShouldBeTrue();
            result.ErrorMessages.ShouldNotBeNull();
            result.ErrorMessages.ShouldBeEmpty();
        }

        [Test]
        public void Should_Return_Invalid_When_String_Lenghts_Dont_Match() {
            // Arrange
            var validator = new StringLengthValidator(40);

            // Act
            var result = validator.IsValid(new string('A', 39));

            // Assert
            result.ShouldNotBeNull();
            result.Valid.ShouldBeFalse();
            result.ErrorMessages.ShouldNotBeNull();
            result.ErrorMessages.Count.ShouldEqual(1);
            result.ErrorMessages.Single().ParamName.ShouldEqual("input");
            result.ErrorMessages.Single().Message.ShouldEqual("Length of input string does not match expected. Expected: 40. Actual: 39.");
        }
    }
}
