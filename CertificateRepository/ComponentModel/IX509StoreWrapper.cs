using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace CertificateRepository.ComponentModel {
    public interface IX509StoreWrapper {
        void Open(StoreName storeName, StoreLocation storeLocation, OpenFlags openFlags);
        IEnumerable<X509Certificate2> Find(X509FindType findType, string criteria, bool validOnly);
        void Close();
    }
}