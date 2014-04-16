using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using CertificateRepository.IntegrationTests.Annotations;
using NUnit.Framework;
using Should;
using TechTalk.SpecFlow;

namespace CertificateRepository.IntegrationTests.Steps {
    [Binding, UsedImplicitly]
    public class SharedSteps : StepBase {
        private const string CertificateNotFoundErrorMessage =
            "Unable to find {0} in Certificates folder, check the following:{1}"
            + " 1. {0} has been generated.{1}"
            + " 2. The certificate has been added to the solution.{1}"
            + " 3. The certificate has had it's properties set to 'Copy if newer' or 'Copy always'.{1}{1}"
            + " Locations Searched:{1} - {2}";

        [Given(@"the certificate '(.*)' has been loaded using password '(.*)'"), UsedImplicitly]
        public void GivenTheCertificateHasBeenLoadedUsingPassword(string certificateFile, string password) {
            var certificateFolder = Path.GetFullPath(".\\Certificates");
            var certificatePath = Path.Combine(certificateFolder, certificateFile);

            if (!File.Exists(certificatePath)) {
                Assert.Fail(CertificateNotFoundErrorMessage, certificateFile, Environment.NewLine, certificateFolder);
            }

            var certificate = new X509Certificate2(certificatePath, password);
            Context.Certificate = certificate;
        }

        [Given(@"place it into the '(.*)' store for the '(.*)'"), UsedImplicitly]
        public void GivenPlacedIntoTheStoreForThe(string storeName, string storeLocation) {
            var store = new X509Store(storeName, ParseEnum<StoreLocation>(storeLocation));
            store.Open(OpenFlags.ReadWrite);
            store.Add(Context.Certificate);

            Context.CertificatesToCleanup.Add(Context.Certificate);
            Context.StoreName = storeName;
            Context.StoreLocation = storeLocation;
        }

        [Then(@"any certificates should be cleaned up"), UsedImplicitly]
        public void ThenTheCertificateShouldBeCleanedUp() {
            CleanupCertificates();
        }

        [Then(@"there should only be '1' certificate in the collection"), UsedImplicitly]
        public void ThenThereShouldOnlyBe1CertificateInTheCollection() {
            Context.Results.Count().ShouldEqual(1);
        }
        
        [Then(@"there should be '(.*)' certificates in the collection"), UsedImplicitly]
        public void ThenThereShouldOnlyBeNCertificateInTheCollection(int expectedCount) {
            Context.Results.Count().ShouldEqual(expectedCount, GetCertificateErrorMessage());
        }

        [Then(@"there should be '(.*)' or more certificates in the collection"), UsedImplicitly]
        public void ThenThereShouldBeOrMoreCertificatesInTheCollection(int expectedCount) {
            Context.Results.Count().ShouldBeGreaterThan(expectedCount);
        }

        [Then(@"one certificate retrieved should match the thumbprint '(.*)'"), UsedImplicitly]
        public void ThenOnceCertificateRetrievedShouldMatchTheThumprint(string thumbprint) {
            Context.Results.Select(x => x.Thumbprint).ShouldContain(thumbprint);
        }

        [AfterScenario("CleanupCertificatesOnError"), UsedImplicitly]
        public void CleanupCertificatesOnError() {
            if (ScenarioContext.Current.TestError == null) {
                return;
            }

            CleanupCertificates();
        }

        private void CleanupCertificates() {
            if (Context.CertificatesToCleanup == null) {
                return;
            }

            var store = new X509Store(Context.StoreName, ParseEnum<StoreLocation>(Context.StoreLocation));
            store.Open(OpenFlags.ReadWrite);
            foreach (var certificate in Context.CertificatesToCleanup) {
                store.Remove(certificate);
            }
            store.Close();
        }

        private string GetCertificateErrorMessage() {
            var builder = new StringBuilder();
            
            builder.AppendFormat("The current state of the {0} store in the {1} location:", Context.StoreName, Context.StoreLocation).AppendLine();

            var store = new X509Store(Context.StoreName, ParseEnum<StoreLocation>(Context.StoreLocation));
            store.Open(OpenFlags.ReadWrite);
            var certificates = store.Certificates.Cast<X509Certificate2>().ToArray();
            var format = "{0,-40} {1,-" + certificates.Max(x => x.SubjectName.Name != null ? x.SubjectName.Name.Length : 0) + "}";
            builder.AppendFormat(format, "Thumbprint", "Subject Name").AppendLine();
            foreach (var certificate in certificates) {
                builder.AppendFormat(format, certificate.Thumbprint, certificate.SubjectName.Name).AppendLine();
            }
            store.Close();
            return builder.ToString();
        }
    }
}