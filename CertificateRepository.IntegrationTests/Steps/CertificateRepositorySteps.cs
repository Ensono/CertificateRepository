using System.Security.Cryptography.X509Certificates;
using CertificateRepository.IntegrationTests.Annotations;
using TechTalk.SpecFlow;

namespace CertificateRepository.IntegrationTests.Steps {
    [Binding, UsedImplicitly]
    public class CertificateRepositorySteps : StepBase {
        [When(@"I create a certificate repository"), UsedImplicitly]
        public void WhenICreateACertificateRepository() {
            Context.Repository = new CertificateRepository();
        }

        [When(@"set the Store Name to '(.*)'"), UsedImplicitly]
        public void WhenSetTheStoreNameTo(string storeName) {
            Context.Repository.StoreName = ParseEnum<StoreName>(storeName);
        }

        [When(@"set the Store Location to '(.*)'"), UsedImplicitly]
        public void WhenSetTheStoreLocationTo(string storeLocation) {
            Context.Repository.StoreLocation = ParseEnum<StoreLocation>(storeLocation);
        }

        [When(@"I call FindBySubjectName with subject '(.*)'"), UsedImplicitly]
        public void WhenICallFindBySubjectNameWithSubject(string subject) {
            Context.Results = Context.Repository.FindBySubjectName(subject, validOnly: false);
        }

        [When(@"^I call FindByThumbprint with thumbprint '([A-Fa-f0-0]*)'$"), UsedImplicitly]
        public void WhenICallFindCertificateByThumbprint(string thumbprint) {
            var result = Context.Repository.FindByThumbprint(thumbprint, validOnly: false);
            Context.Results = new[] {result};
        }

        [When(@"I call FindBySubjectDistinguishedName with subject '(.*)'"), UsedImplicitly]
        public void WhenICallFindBySubjectDistinguishedNameWithSubject(string distinguishedName) {
            Context.Results = Context.Repository.FindBySubjectDistinguishedName(distinguishedName, validOnly: false);
        }
    }
}