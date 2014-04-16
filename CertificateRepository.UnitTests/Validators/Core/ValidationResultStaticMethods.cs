using System.Linq;
using CertificateRepository.Validators.Core;
using NUnit.Framework;
using Should;

namespace CertificateRepository.UnitTests.Validators.Core {
    [TestFixture]
    public class ValidationResultStaticMethods {
        [Test]
        public void Should_Return_Successful_Result() {
            // Act
            var result = ValidationResult.Success();

            // Assert
            result.Valid.ShouldBeTrue();
            result.ErrorMessages.ShouldNotBeNull();
            result.ErrorMessages.ShouldBeEmpty();
        }

        [Test]
        public void Should_Return_Failure_Result() {
            // Act
            var result = ValidationResult.Failure("Message", "ParamName");

            // Assert
            result.Valid.ShouldBeFalse();
            result.ErrorMessages.ShouldNotBeNull();
            result.ErrorMessages.Count.ShouldEqual(1);
            result.ErrorMessages.First().Message.ShouldEqual("Message");
            result.ErrorMessages.First().ParamName.ShouldEqual("ParamName");
        }
    }
}
