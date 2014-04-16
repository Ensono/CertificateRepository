using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using CertificateRepository.Wrappers;
using TechTalk.SpecFlow;

namespace CertificateRepository.IntegrationTests.Infrastructure {
    public class ScenarioContextWrapper {
        private readonly ScenarioContext _context = ScenarioContext.Current;

        public X509Certificate2 Certificate
        {
            get { return Get<X509Certificate2>("Certificate"); }
            set { Set("Certificate", value); }
        }

        public string StoreName {
            get { return Get<string>("StoreName"); }
            set { Set("StoreName", value); }
        }

        public string StoreLocation {
            get { return Get<string>("StoreLocation"); }
            set { Set("StoreLocation", value); }
        }

        public X509StoreWrapper Wrapper {
            get { return Get<X509StoreWrapper>("Wrapper"); }
            set { Set("Wrapper", value); }
        }

        public IEnumerable<X509Certificate2> Results {
            get { return Get<IEnumerable<X509Certificate2>>("Results"); }
            set { Set("Results", value); }
        }

        public CertificateRepository Repository {
            get { return Get<CertificateRepository>("Repository"); }
            set { Set("Repository", value); }
        }

        public IList<X509Certificate2> CertificatesToCleanup {
            get { return NewOrGet<List<X509Certificate2>>("CertificatesToCleanup"); }
            set { Set("CertificatesToCleanup", value); }
        }

        public Exception Exception {
            get { return Get<Exception>("Exception"); }
            set { Set("Exception", value); }
        }

        private TType NewOrGet<TType>(string key) where TType : new() {
            if (!_context.ContainsKey(key)) {
                var value = new TType();
                _context.Add(key, value);
            }

            return _context.Get<TType>(key);
        }

        private TType Get<TType>(string key) {
            if (!_context.ContainsKey(key)) {
                TType value = default(TType);
                _context.Add(key, value);
            }

            return _context.Get<TType>(key);
        }

        private void Set<TType>(string key, TType value)
        {
            if (_context.ContainsKey(key)) {
                _context.Remove(key);
            }

            _context.Add(key, value);
        }
    }
}