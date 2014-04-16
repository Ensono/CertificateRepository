using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using CertificateRepository.ComponentModel;
using CertificateRepository.Exceptions;
using CertificateRepository.Validators.Core;
using Moq;
using NUnit.Framework;
using Should;

namespace CertificateRepository.UnitTests {
    [TestFixture]
    public class CertificateRepositoryTests {
        private Mock<IX509StoreWrapper> _storeWrapper;
        private Mock<IValidator<string>> _thumbprintValidator;

        private class StubX509Certificate2 : X509Certificate2 {
            public StubX509Certificate2(X500DistinguishedName subjectName) {
                SubjectName = subjectName;
            }

            public StubX509Certificate2(string thumbprint) {
                Thumbprint = thumbprint;
            }

            public new X500DistinguishedName SubjectName { get; private set; }
            public new string Thumbprint { get; private set; }
        }

        private CertificateRepository GetRepository(StoreName storeName = StoreName.My, StoreLocation storeLocation = StoreLocation.CurrentUser) {
            return new CertificateRepository(_storeWrapper.Object, _thumbprintValidator.Object) {
                StoreName = storeName,
                StoreLocation = storeLocation
            };
        }

        [SetUp]
        public void SetUp() {
            _thumbprintValidator = new Mock<IValidator<string>>();
            _thumbprintValidator.Setup(x => x.IsValid("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"))
                .Returns(new ValidationResult { ErrorMessages = new List<ValidationMessage>(), Valid = true });

            _storeWrapper = new Mock<IX509StoreWrapper>();
            _storeWrapper.Setup(x => x.Open(StoreName.My, StoreLocation.CurrentUser, OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly));
            _storeWrapper.Setup(x => x.Close());
        }

        [Test]
        public void Should_Throw_ArgumentNullException_If_Wrapper_Is_Null() {
            // Arrange
            var thumbprintValidator = new Mock<IValidator<string>>();
            TestDelegate sit = () => new CertificateRepository(null, thumbprintValidator.Object);

            // Act & Assert
            var anex = Assert.Throws<ArgumentNullException>(sit);
            anex.ParamName.ShouldEqual("storeWrapper");
        }

        [Test]
        public void Should_Throw_ArgumentNullException_If_ThumbprintValidator_Is_Null() {
            // Arrange
            var x509StoreWrapper = new Mock<IX509StoreWrapper>();
            TestDelegate sit = () => new CertificateRepository(x509StoreWrapper.Object, null);

            // Act & Assert
            var anex = Assert.Throws<ArgumentNullException>(sit);
            anex.ParamName.ShouldEqual("thumbprintValidator");
        }

        [Test]
        public void Should_Open_Certificate_Store() {
            // Arrange
            _storeWrapper.Setup(x => x.Find(X509FindType.FindBySubjectName, "Sample Certificate", false))
                .Returns(new List<X509Certificate2>());

            // Act
            GetRepository().FindBySubjectName("Sample Certificate", validOnly: false);

            // Assert
            _storeWrapper.Verify(x => x.Open(StoreName.My, StoreLocation.CurrentUser, OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly), Times.Once);
        }

        [Test]
        public void Should_Call_Find_Certificate_With_FindBySubjectName() {
            // Arrange
            _storeWrapper.Setup(x => x.Find(X509FindType.FindBySubjectName, "Sample Certificate", false))
                .Returns(new List<X509Certificate2>());

            // Act
            GetRepository().FindBySubjectName("Sample Certificate", validOnly: false);

            // Assert
            _storeWrapper.Verify(x => x.Find(X509FindType.FindBySubjectName, "Sample Certificate", false), Times.Once);
        }

        [Test]
        public void Should_Call_Find_Certificate_With_FindByThumbprint() {
            // Arrange
            _thumbprintValidator.Setup(x => x.IsValid("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"))
                .Returns(new ValidationResult {ErrorMessages = new List<ValidationMessage>(), Valid = true});
            _storeWrapper.Setup(x => x.Open(StoreName.TrustedPeople, StoreLocation.CurrentUser, OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly));
            var certificates = new[] {new StubX509Certificate2(new string('A', 40))};
            _storeWrapper.Setup(x => x.Find(X509FindType.FindByThumbprint, "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", false))
                .Returns(certificates);

            // Act
            GetRepository(StoreName.TrustedPeople).FindByThumbprint("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", validOnly: false);

            // Assert
            _storeWrapper.Verify(x => x.Find(X509FindType.FindByThumbprint, "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", false), Times.Once);
        }

        [Test]
        public void Should_Call_Find_Certificate_With_FindBySubjectDistinguishedName() {
            // Arrange
            _storeWrapper.Setup(x => x.Open(StoreName.TrustedPeople, StoreLocation.CurrentUser, OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly));
            var certificates = new[] { new StubX509Certificate2(new X500DistinguishedName("CN=A")) };
            _storeWrapper.Setup(x => x.Find(X509FindType.FindBySubjectDistinguishedName, "CN=A", false))
                .Returns(certificates);

            // Act
            GetRepository(StoreName.TrustedPeople).FindBySubjectDistinguishedName("CN=A", validOnly: false);

            // Assert
            _storeWrapper.Verify(x => x.Find(X509FindType.FindBySubjectDistinguishedName, "CN=A", false), Times.Once);
        }

        [Test]
        public void Should_Return_One_Certificate_With_FindBySubjectDistinguishedName() {
            // Arrange
            _storeWrapper.Setup(x => x.Open(StoreName.TrustedPeople, StoreLocation.CurrentUser, OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly));
            var certificates = new[] { new StubX509Certificate2(new X500DistinguishedName("CN=A")) };
            _storeWrapper.Setup(x => x.Find(X509FindType.FindBySubjectDistinguishedName, "CN=A", false))
                .Returns(certificates);

            // Act
            var results = GetRepository(StoreName.TrustedPeople).FindBySubjectDistinguishedName("CN=A", validOnly: false);

            // Assert
            results.ShouldImplement<IEnumerable<X509Certificate2>>();
            var certs = results as IList<X509Certificate2> ?? results.ToList();
            certs.Count().ShouldEqual(1);
            certs.Cast<StubX509Certificate2>().Single().SubjectName.Name.ShouldEqual("CN=A");
        }

        [Test]
        public void Should_Return_Two_Certificates_When_Calling_FindBySubjectName() {
            // Arrange
            var certificates = new[] {
                new StubX509Certificate2(new X500DistinguishedName("CN=1")),
                new StubX509Certificate2(new X500DistinguishedName("CN=2"))
            };
            _storeWrapper.Setup(x => x.Find(X509FindType.FindBySubjectName, "Sample Certificate", false)).Returns(certificates);

            // Act
            var result = GetRepository().FindBySubjectName("Sample Certificate", validOnly: false);

            // Assert
            var results = result as X509Certificate2[] ?? result.ToArray();
            results.Count().ShouldEqual(2);
            results.Cast<StubX509Certificate2>().First().SubjectName.Name.ShouldEqual("CN=1");
            results.Cast<StubX509Certificate2>().Last().SubjectName.Name.ShouldEqual("CN=2");
        }

        [Test]
        public void Should_Return_Certificate_When_Find_By_Thumbprint_Called() {
            // Arrange
            _storeWrapper.Setup(x => x.Open(StoreName.TrustedPeople, StoreLocation.CurrentUser, OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly));
            var certificates = new[] { new StubX509Certificate2(new string('A', 40)) };
            _storeWrapper.Setup(x => x.Find(X509FindType.FindByThumbprint, "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", false))
                .Returns(certificates);

            // Act
            var result = GetRepository(StoreName.TrustedPeople).FindByThumbprint("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", validOnly: false);

            // Assert
            var resultActual = result as StubX509Certificate2;
            resultActual.ShouldNotBeNull();
            resultActual.Thumbprint.ShouldEqual("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
        }

        [Test]
        public void Should_Open_Store_In_LocalMachine() {
            // Arrange
            var certificates = new[] {
                new StubX509Certificate2(new X500DistinguishedName("CN=1")), 
                new StubX509Certificate2(new X500DistinguishedName("CN=2"))
            };
            _storeWrapper.Setup(x => x.Find(X509FindType.FindBySubjectName, "Sample Certificate", false)).Returns(certificates);

            GetRepository(storeLocation: StoreLocation.LocalMachine).FindBySubjectName("Sample Certificate", validOnly: false);

            // Assert
            _storeWrapper.Verify(x => x.Open(StoreName.My, StoreLocation.LocalMachine, OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly), Times.Once);
        }

        [Test]
        public void Should_Open_Trusted_People_Store() {
            // Arrange
            _storeWrapper.Setup(x => x.Open(StoreName.TrustedPeople, StoreLocation.CurrentUser, OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly));
            var certificates = new[]
            {new StubX509Certificate2(new X500DistinguishedName("CN=1")), new StubX509Certificate2(new X500DistinguishedName("CN=2"))};
            _storeWrapper.Setup(x => x.Find(X509FindType.FindBySubjectName, "Sample Certificate", false)).Returns(certificates);

            // Act
            GetRepository(StoreName.TrustedPeople).FindBySubjectName("Sample Certificate", validOnly: false);

            // Assert
            _storeWrapper.Verify(x => x.Open(StoreName.TrustedPeople, StoreLocation.CurrentUser, OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly), Times.Once);
        }

        [Test]
        public void Should_Parse_TrustedPeople_StoreName() {
            // Arrange
            var repository = new CertificateRepository(_storeWrapper.Object, _thumbprintValidator.Object);

            // Act
            repository.SetStoreName("TrustedPeople");

            // Assert
            repository.StoreName.ShouldEqual(StoreName.TrustedPeople);
        }

        [Test]
        public void Should_Parse_TrustedPeople_LowerCase_StoreName() {
            // Arrange
            var repository = new CertificateRepository(_storeWrapper.Object, _thumbprintValidator.Object);

            // Act
            repository.SetStoreName("trustedpeople");

            // Assert
            repository.StoreName.ShouldEqual(StoreName.TrustedPeople);
        }

        [Test]
        public void Should_Throw_If_Unable_To_Parse_StoreName() {
            // Arrange
            var repository = new CertificateRepository(_storeWrapper.Object, _thumbprintValidator.Object);

            // Act
            TestDelegate sit = () => repository.SetStoreName("xyz");

            // Assert
            Assert.Throws<UnableToParseArgumentException>(sit);
        }

        [Test]
        public void Should_Parse_LocalMachine_StoreLocation() {
            // Arrange
            var repository = new CertificateRepository(_storeWrapper.Object, _thumbprintValidator.Object);

            // Act
            repository.SetStoreLocation("LocalMachine");

            // Assert
            repository.StoreLocation.ShouldEqual(StoreLocation.LocalMachine);
        }

        [Test]
        public void Should_Parse_LocalMachine_LowerCase_StoreLocation() {
            // Arrange
            var repository = new CertificateRepository(_storeWrapper.Object, _thumbprintValidator.Object);

            // Act
            repository.SetStoreLocation("localmachine");

            // Assert
            repository.StoreLocation.ShouldEqual(StoreLocation.LocalMachine);
        }

        [Test]
        public void Should_Throw_If_Unable_To_Parse_StoreLocation() {
            // Arrange
            var repository = new CertificateRepository(_storeWrapper.Object, _thumbprintValidator.Object);

            // Act
            TestDelegate sit = () => GetRepository().SetStoreLocation("xyz");

            // Assert
            Assert.Throws<UnableToParseArgumentException>(sit);
        }

        [Test]
        public void Should_Throw_CertificateException_In_Underlying_Wrapper_Throws_SecurityException() {
            // Arrange
            _storeWrapper.Setup(x => x.Open(StoreName.My, StoreLocation.CurrentUser, OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly)).Throws<SecurityException>();

            // Act
            TestDelegate sit = () => GetRepository().FindBySubjectName("blah", validOnly: false);

            // Assert
            Assert.Throws<CertificateException>(sit);
        }

        [Test]
        public void Should_Throw_CertificateException_In_Underlying_Wrapper_Throws_CryptographicException() {
            // Arrange
            _storeWrapper.Setup(x => x.Open(StoreName.My, StoreLocation.CurrentUser, OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly)).Throws<CryptographicException>();

            // Act
            TestDelegate sit = () => GetRepository().FindBySubjectName("blah", validOnly: false);

            // Assert
            Assert.Throws<CertificateException>(sit);
        }

        [Test]
        public void Should_Throw_ArgumentNullException_When_StoreName_Null() {
            // Arrange
            _storeWrapper.Setup(x => x.Open(StoreName.My, StoreLocation.CurrentUser, OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly)).Throws<CryptographicException>();

            // Act
            TestDelegate sit = () => GetRepository().SetStoreName(null);

            // Assert
            Assert.Throws<ArgumentNullException>(sit);
        }

        [Test]
        public void Should_Throw_ArgumentNullException_When_StoreLocation_Null() {
            // Arrange
            _storeWrapper.Setup(x => x.Open(StoreName.My, StoreLocation.CurrentUser, OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly)).Throws<CryptographicException>();

            // Act
            TestDelegate sit = () => GetRepository().SetStoreLocation(null);

            // Assert
            Assert.Throws<ArgumentNullException>(sit);
        }

        [Test]
        public void Should_Close_Store_In_Happy_Path() {
            // Arrange
            var certificates = new[] { new StubX509Certificate2(new X500DistinguishedName("CN=1")) };
            _storeWrapper.Setup(x => x.Find(X509FindType.FindBySubjectName, "1", false)).Returns(certificates);

            // Act
            GetRepository().FindBySubjectName("1", validOnly: false);

            // Assert
            _storeWrapper.Verify(x => x.Close(), Times.Once);
        }

        [Test]
        public void Should_Close_Store_In_Not_Happy_Path() {
            // Arrange
            _storeWrapper.Setup(x => x.Find(X509FindType.FindBySubjectName, "1", false)).Throws<SecurityException>();

            // Act
            try {
                // swallow the exception
                GetRepository().FindBySubjectName("1", validOnly: false);
            }
            catch (Exception ex) {
                Trace.WriteLine(string.Format("Disgarded Exception: {0}", ex));
            }

            // Assert
            _storeWrapper.Verify(x => x.Close(), Times.Once);
        }
    }
}