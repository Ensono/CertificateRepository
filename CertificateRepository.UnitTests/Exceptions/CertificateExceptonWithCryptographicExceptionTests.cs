using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using CertificateRepository.Exceptions;
using NUnit.Framework;
using Should;

namespace CertificateRepository.UnitTests.Exceptions {
    [TestFixture]
    public class CertificateExceptonWithCryptographicExceptionTests {
        [Test]
        public void Should_Include_StoreName_In_Message() {
            // Arrange
            var cryptographicException = new CryptographicException();

            // Act
            var certificateException = new CertificateException("Operation", StoreName.TrustedPeople, StoreLocation.CurrentUser, cryptographicException);

            // Assert
            certificateException.Message.ShouldContain("TrustedPeople");
        }

        [Test]
        public void Should_Include_StoreLocation_In_Message() {
            // Arrange
            var cryptographicException = new CryptographicException();

            // Act
            var certificateException = new CertificateException("Opperation", StoreName.TrustedPeople, StoreLocation.CurrentUser, cryptographicException);

            // Assert
            certificateException.Message.ShouldContain("CurrentUser");
        }

        [Test]
        public void Should_Include_InnerException_Message_In_Message() {
            // Arrange
            var cryptographicException = new CryptographicException("Message");

            // Act
            var certificateException = new CertificateException("Opperation", StoreName.TrustedPeople, StoreLocation.CurrentUser, cryptographicException);

            // Assert
            certificateException.Message.ShouldContain("Message");
        }

        [Test]
        public void Should_Include_Operation_In_Message() {
            // Arrange
            var cryptographicException = new CryptographicException("Message");

            // Act
            var certificateException = new CertificateException("Opperation", StoreName.TrustedPeople, StoreLocation.CurrentUser, cryptographicException);

            // Assert
            certificateException.Message.ShouldContain("Message");
        }

        [Test]
        public void Should_Have_A_Descriptive_Message() {
            // Arrange
            var cryptographicException = new CryptographicException("Message");
            var messageRegex = new Regex("A cryptographic error was encountered completing the (.+) operation on the (.+) store in the (.+) location: (.+)", RegexOptions.Compiled);

            // Act
            var certificateException = new CertificateException("Opperation", StoreName.TrustedPeople, StoreLocation.CurrentUser, cryptographicException);

            // Assert
            messageRegex.IsMatch(certificateException.Message).ShouldBeTrue();
        }

        [Test]
        public void Should_Set_InnerException() {
            // Arrange
            var cryptographicException = new CryptographicException("Message");

            // Act
            var certificateException = new CertificateException("Opperation", StoreName.TrustedPeople, StoreLocation.CurrentUser, cryptographicException);

            // Assert
            certificateException.InnerException.ShouldNotBeNull();
            certificateException.InnerException.ShouldEqual(cryptographicException);
        }
    }
}