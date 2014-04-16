using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using CertificateRepository.ComponentModel;

namespace CertificateRepository.Wrappers {
    public class X509StoreWrapper : IX509StoreWrapper {
        private X509Store _store;

        public void Open(StoreName storeName, StoreLocation storeLocation, OpenFlags openFlags) {
            _store = new X509Store(storeName, storeLocation);
            _store.Open(openFlags);
        }

        public IEnumerable<X509Certificate2> Find(X509FindType findType, string criteria, bool validOnly) {
            return _store.Certificates.Find(findType, criteria, validOnly)
                .Cast<X509Certificate2>()
                .AsEnumerable();
        }

        public void Close() {
            _store.Close();
        }
    }
}