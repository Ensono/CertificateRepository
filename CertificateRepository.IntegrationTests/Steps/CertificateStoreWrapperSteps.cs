using System;
using System.Security.Cryptography.X509Certificates;
using CertificateRepository.IntegrationTests.Annotations;
using CertificateRepository.Wrappers;
using TechTalk.SpecFlow;

namespace CertificateRepository.IntegrationTests.Steps {
    [Binding, UsedImplicitly]
    public class CertificateStoreWrapperSteps : StepBase {
        [When(@"I create a store wrapper"), UsedImplicitly]
        public void WhenICreateAStoreWrapperUsingTheStoreForThe() {
            Context.Wrapper = new X509StoreWrapper();
        }

        [When(@"I open the '(.*)' store for the '(.*)' with the '(.*)' flag"), UsedImplicitly]
        public void WhenIOpenTheStoreWithTheFlag(string storeName, string storeLocation, string flag) {
            var wrapper = Context.Wrapper;
            wrapper.Open(ParseEnum<StoreName>(storeName), ParseEnum<StoreLocation>(storeLocation), ParseEnum<OpenFlags>(flag));
            Context.Wrapper = wrapper;
        }

        [When(@"I call find certificate by (.*) '(.*)'"), UsedImplicitly]
        public void WhenICallFindCertificateBy(string methodName, string parameter) {
            var wrapper = Context.Wrapper;
            var method = ParseEnum<X509FindType>(string.Format("FindBy{0}", methodName.Replace(" ", String.Empty)));
            var certificates = wrapper.Find(method, parameter, false);
            Context.Results = certificates;
        }
    }
}
