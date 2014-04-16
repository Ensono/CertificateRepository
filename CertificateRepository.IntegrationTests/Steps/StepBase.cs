using System;
using CertificateRepository.IntegrationTests.Infrastructure;

namespace CertificateRepository.IntegrationTests.Steps {
    public class StepBase {
        protected StepBase() {
            Context = new ScenarioContextWrapper();
        }

        protected ScenarioContextWrapper Context { get; private set; }

        protected TEnum ParseEnum<TEnum>(string storeLocation) {
            const bool ignoreCase = true;
            return (TEnum) Enum.Parse(typeof (TEnum), storeLocation, ignoreCase);
        }
    }
}