using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using CertificateRepository.Annotations;
using CertificateRepository.ComponentModel;
using CertificateRepository.Exceptions;
using CertificateRepository.Validators;
using CertificateRepository.Wrappers;

namespace CertificateRepository {
    public class CertificateRepository {
        private readonly IX509StoreWrapper _storeWrapper;
        private readonly IValidator<string> _thumbprintValidator; 
        private StoreName _storeName = StoreName.My;
        private StoreLocation _storeLocation = StoreLocation.CurrentUser;
        private const bool IgnoreCase = true;
        private const OpenFlags ReadOnlyOpenFlags = OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly;

        private void OpenStore(OpenFlags openFlags) {
            try {
                _storeWrapper.Open(_storeName, _storeLocation, openFlags);
            }
            catch (SecurityException securityException) {
                throw new CertificateException(_storeName, _storeLocation, securityException);
            }
            catch (CryptographicException cryptographicException) {
                throw new CertificateException("open", _storeName, _storeLocation, cryptographicException);
            }
        }

        private IEnumerable<X509Certificate2> Find(X509FindType findType, string criteria, bool validOnly) {
            try {
                return _storeWrapper.Find(findType, criteria, validOnly);
            } finally {
                if (_storeWrapper != null) {
                    _storeWrapper.Close();
                }
            }
        }

        /// <summary>
        /// Default parameterless constructor; uses the built in instance of <see cref="X509StoreWrapper" />.
        /// </summary>
        /// <example>
        /// var certificateRepo = new CertificateRepository();
        /// </example>
        [UsedImplicitly]
        public CertificateRepository() : this(
            new X509StoreWrapper(), 
            new ThumbprintValidator(
                new ThumbprintBeginsWithLeftToRightCharacterValidator(),
                new ThumbprintEndsWithLeftToRightCharacterValidator(),
                new ThumbprintStringValidator(new ThumbprintCharacterValidator(), new ThumbprintStringLengthValidator()))
            ) { }

        /// <summary>
        /// Dependency Injection aware constructor; uses the instance of <see cref="IX509StoreWrapper" />
        /// </summary>
        /// <example>
        /// var storeWrapper = new X509StoreWrapper();
        /// var certificateRepo = new CertificateRepository(storeWrapper);
        /// </example>
        /// <remarks>
        /// This constructor should only be used if you need control over the <see cref="IX509StoreWrapper" /> 
        /// </remarks>
        /// <param name="storeWrapper">Instance of <see cref="IX509StoreWrapper" /></param>
        /// <param name="thumbprintValidator">Validator that implements <see cref="IValidator" /> and asserts that a thumbprint is valid.</param>
        /// <exception cref="ArgumentNullException">Thrown in the event that <paramref name="storeWrapper"/> or <paramref name="thumbprintValidator"/> is null.</exception>
        public CertificateRepository(IX509StoreWrapper storeWrapper, IValidator<string> thumbprintValidator) {
            if (storeWrapper == null) {
                throw new ArgumentNullException("storeWrapper");
            }

            if (thumbprintValidator == null) {
                throw new ArgumentNullException("thumbprintValidator");
            }

            _storeWrapper = storeWrapper;
            _thumbprintValidator = thumbprintValidator;
        }

        /// <summary>
        /// Set the store name with one of the known <see cref="StoreName" /> values.
        /// </summary>
        public StoreName StoreName {
            get {
                return _storeName;
            }
            set {
                _storeName = value;
            }
        }

        /// <summary>
        /// Set the store location with one of the known <see cref="StoreLocation" /> values.
        /// </summary>
        public StoreLocation StoreLocation {
            get {
                return _storeLocation;
            }
            set {
                _storeLocation = value;
            }
        }

        /// <summary>
        /// Set the store name from a string, will be parsed into one of the known <see cref="StoreName" /> values.
        /// </summary>
        /// <param name="storeName">String representation of one of the known <see cref="StoreName" /> types; case insensitive.</param>
        /// <exception cref="UnableToParseArgumentException">Thrown if the Store Name cannot be parsed, inner exception will contain detailed explanation of failure.</exception>
        public void SetStoreName(string storeName) {
            if (storeName == null) {
                throw new ArgumentNullException("storeName");
            }

            if (Enum.TryParse(storeName, IgnoreCase, out _storeName)) {
                return;
            }

            throw new UnableToParseArgumentException("storeName", storeName, typeof (StoreName));
        }

        /// <summary>
        /// Set the store location from a string, will be parsed into one of the known <see cref="StoreLocation" /> values.
        /// </summary>
        /// <param name="storeLocation">String representation of one of the known <see cref="StoreLocation" /> types; case insensitive.</param>
        /// <exception cref="UnableToParseArgumentException">Thrown if the Store Location cannot be parsed, inner exception will contain detailed explanation of failure.</exception>
        public void SetStoreLocation(string storeLocation) {
            if (storeLocation == null) {
                throw new ArgumentNullException("storeLocation");
            }

            if (Enum.TryParse(storeLocation, IgnoreCase, out _storeLocation)) {
                return;
            }

            throw new UnableToParseArgumentException("storeLocation", storeLocation, typeof (StoreLocation));
        }

        /// <summary>
        /// Finds a certificate by the subject name of the certificate, partial match.
        /// </summary>
        /// <param name="subjectName">Partial match to the subject name.</param>
        /// <param name="validOnly">If True then will only return valid certificates, otherwise will return all matching certificates.</param>
        /// <returns>The result of the query returned as an enumeration of <see cref="X509Certificate2"/> certificates.</returns>
        /// <remarks>
        /// Please note that calls to FindBySubjectName will match any part of the certificates subject, thus if 
        /// you have a certificate with a Subject of CN=RP (Relying Party) and another with a Subject of CN=RPSTS
        /// (Relying Party Security Token Service) then both will be matched when calling
        /// <code>certificateRepository.FindBySubjectName("RP")</code>
        /// </remarks>
        public IEnumerable<X509Certificate2> FindBySubjectName(string subjectName, bool validOnly = true) {
            OpenStore(ReadOnlyOpenFlags);
            return Find(X509FindType.FindBySubjectName, subjectName, validOnly);
        }

        /// <summary>
        /// Finds a certificate by the SHA1 thumbprint of the the certificate.
        /// </summary>
        /// <param name="thumbprint">The Thumbprint</param>
        /// <param name="validOnly">If True then will only return valid certificates, otherwise will return all matching certificates.</param>
        /// <returns>The result of the query returned as an enumeration of <see cref="X509Certificate2"/> certificates.</returns>
        public X509Certificate2 FindByThumbprint(string thumbprint, bool validOnly = true) {
           var validationResult = _thumbprintValidator.IsValid(thumbprint);

            if (!validationResult.Valid) {
                throw new CompositeArgumentException(validationResult.ErrorMessages);
            } 

            OpenStore(ReadOnlyOpenFlags);
            var result = Find(X509FindType.FindByThumbprint, thumbprint, validOnly);
            return result == null ? null : result.SingleOrDefault();
        }

        /// <summary>
        /// Finds a certficate by the subject name of the certificate, exact match.
        /// </summary>
        /// <param name="distinguishedName">The Subject Name expressed as a X.500 Distintinguished Name.</param>
        /// <param name="validOnly">If True then will only return valid certificates, otherwise will return all matching certificates.</param>
        /// <returns>The result of the query returned as an enumeration of <see cref="X509Certificate2"/> certificates.</returns>
        public IEnumerable<X509Certificate2> FindBySubjectDistinguishedName(string distinguishedName, bool validOnly = true) {
            OpenStore(ReadOnlyOpenFlags);
            return Find(X509FindType.FindBySubjectDistinguishedName, distinguishedName, validOnly);
        }
    }
}
