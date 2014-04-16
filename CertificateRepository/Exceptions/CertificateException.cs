using System;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace CertificateRepository.Exceptions {
    [Serializable]
    public class CertificateException : Exception {
        private const string SecurityMessageFormat = "Unable to access the {0} store in the {1} location: {2}";
        private const string CryptographicMessageFormat = "A cryptographic error was encountered completing the {3} operation on the {0} store in the {1} location: {2}";

        public CertificateException(StoreName storeName, StoreLocation storeLocation, SecurityException innerException)
            : base(string.Format(SecurityMessageFormat, storeName, storeLocation, innerException.Message), innerException) { }

        public CertificateException(string operation, StoreName storeName, StoreLocation storeLocation,
            CryptographicException innerException)
            : base(string.Format(CryptographicMessageFormat, storeName, storeLocation, innerException.Message, operation), innerException) { }
    }
}
