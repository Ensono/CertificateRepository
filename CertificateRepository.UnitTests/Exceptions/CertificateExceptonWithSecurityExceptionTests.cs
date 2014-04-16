using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using CertificateRepository.Exceptions;
using NUnit.Framework;
using Should;

namespace CertificateRepository.UnitTests.Exceptions {
    [TestFixture]
    public class CertificateExceptonWithSecurityExceptionTests {
        [Test]
        public void Should_Include_StoreName_In_Message() {
            // Arrange
            var securityException = new SecurityException();

            // Act
            var certificateException = new CertificateException(StoreName.TrustedPeople, StoreLocation.CurrentUser, securityException);

            // Assert
            certificateException.Message.ShouldContain("TrustedPeople");
        }

        [Test]
        public void Should_Include_StoreLocation_In_Message() {
            // Arrange
            var securityException = new SecurityException();

            // Act
            var certificateException = new CertificateException(StoreName.TrustedPeople, StoreLocation.CurrentUser, securityException);

            // Assert
            certificateException.Message.ShouldContain("CurrentUser");
        }

        [Test]
        public void Should_Include_InnerException_Message_In_Message() {
            // Arrange
            var securityException = new SecurityException("Message");

            // Act
            var certificateException = new CertificateException(StoreName.TrustedPeople, StoreLocation.CurrentUser, securityException);

            // Assert
            certificateException.Message.ShouldContain("Message");
        }

        [Test]
        public void Should_Have_A_Descriptive_Message() {
            // Arrange
            var securityException = new SecurityException("Message");
            var messageRegex = new Regex("Unable to access the (.+) store in the (.+) location: (.+)", RegexOptions.Compiled);

            // Act
            var certificateException = new CertificateException(StoreName.TrustedPeople, StoreLocation.CurrentUser, securityException);

            // Assert
            messageRegex.IsMatch(certificateException.Message).ShouldBeTrue();
        }

        [Test]
        public void Should_Set_InnerException() {
            // Arrange
            var securityException = new SecurityException("Message");

            // Act
            var certificateException = new CertificateException(StoreName.TrustedPeople, StoreLocation.CurrentUser, securityException);

            // Assert
            certificateException.InnerException.ShouldNotBeNull();
            certificateException.InnerException.ShouldEqual(securityException);
        }
    }
}
