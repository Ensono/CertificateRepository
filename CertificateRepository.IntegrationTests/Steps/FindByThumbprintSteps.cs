using System;
using System.Collections.Generic;
using System.Linq;
using CertificateRepository.Annotations;
using CertificateRepository.Exceptions;
using NUnit.Framework;
using Should;
using TechTalk.SpecFlow;

namespace CertificateRepository.IntegrationTests.Steps {
    [Binding, UsedImplicitly]
    public class FindByThumbprintSteps : StepBase {
        private readonly Dictionary<string, string> _specialCharacters = new Dictionary<string, string> {
            {"left-to-right-mark", "\u200E"},
            {"space", " "}
        };

        private CompositeArgumentException CompositeArgumentException() {
            var compositeArgumentException = Context.Exception as CompositeArgumentException;
            compositeArgumentException.ShouldNotBeNull();
            compositeArgumentException.InnerExceptions.ShouldNotBeNull();
            return compositeArgumentException;
        }

        private string ApplyCharacter(string thumbprint, string positional, string mark) {
            if (!_specialCharacters.ContainsKey(mark)) {
                throw new ArgumentException(string.Format("Mark ('{0}') is not supported by this class, update _specialCharacters in {1} to add an additional mark.", mark, GetType().Name));
            }

            switch (positional.Normalize().ToLowerInvariant()) {
                case "prefixed":
                    return _specialCharacters[mark] + thumbprint;
                case "sufixed":
                    return thumbprint + _specialCharacters[mark];
                default:
                    throw new ArgumentException("Prefix or Sufix can only be 'prefixed' or 'sufixed'");
            }
        }

        [When(@"I call FindByThumbprint with thumbprint '([A-Fa-f0-9]+)'"), UsedImplicitly]
        public void WhenICallFindByThumbprintWithThumbprint(string thumbprint) {
            var certificate = Context.Repository.FindByThumbprint(thumbprint, validOnly: false);
            Context.Results = new[] {certificate};
        }

        [When(@"I call FindByThumbprint with thumbprint '(.*)' from the following examples"), UsedImplicitly]
        public void WhenICallFindByThumbprintWithThumbprintExpectingException(string thumbprint) {
            FindByThumbprint(thumbprint);
        }

        [When(@"I call FindByThumbprint with thumbprint '([A-Fa-f0-9]+)' (prefixed) with a '([a-z-]+)'"), UsedImplicitly]
        [When(@"I call FindByThumbprint with thumbprint '([A-Fa-f0-9]+)' (sufixed) with a '([a-z-]+)'")]
        public void WhenICallFindByThumbprintWithThumbprintPrefixedWithA(string thumbprint, string operation, string mark) {
            var mangledThumbprint = ApplyCharacter(thumbprint, operation, mark);

            FindByThumbprint(mangledThumbprint);
        }

        private void FindByThumbprint(string mangledThumbprint) {
            try {
                Context.Repository.FindByThumbprint(mangledThumbprint, validOnly: false);
            }
            catch (Exception ex) {
                Context.Exception = ex;
                return;
            }

            Assert.Fail("Exception was expected but not thrown by this step.");
        }

        [Then(@"an exception will be thrown"), UsedImplicitly]
        public void ThenAnExceptionWillBeThrown() {
            Context.Exception.ShouldNotBeNull();
        }

        [Then(@"it will be of type '(.*)'"), UsedImplicitly]
        public void ThenItWillBeOfType(string type) {
            Context.Exception.GetType().Name.ShouldEqual(type);
        }

        [Then(@"the parameter name will be '(.*)'"), UsedImplicitly]
        public void ThenTheParameterNameWillBe(string parameterName) {
            var argumentException = Context.Exception as ArgumentException;
            argumentException.ShouldNotBeNull();
            argumentException.ParamName.ShouldEqual(parameterName);
        }

        [Then(@"it will have ([0-9]+) inner exception"), UsedImplicitly]
        public void ThenItWillHaveInnerException(int expectedCount) {
            var compositeArgumentException = Context.Exception as CompositeArgumentException;
            compositeArgumentException.ShouldNotBeNull();
            compositeArgumentException.InnerExceptions.ShouldNotBeNull();
            compositeArgumentException.InnerExceptions.Count().ShouldEqual(expectedCount);
        }

        [Then(@"inner exception number (.*) will be of type '(.*)'"), UsedImplicitly]
        public void ThenInnerExceptionNumberWillBeOfType(int exceptionNumber, string type) {
            var compositeArgumentException = CompositeArgumentException();

            var exception = compositeArgumentException.InnerExceptions.Skip(exceptionNumber - 1).First();
            exception.GetType().Name.ShouldEqual(type);
        }

        [Then(@"inner exception number (.*) will have a '(.*)' set to '(.*)'"), UsedImplicitly]
        public void ThenInnerExceptionNumberWillHaveASetTo(int exceptionNumber, string property, string value) {
            var compositeArgumentException = CompositeArgumentException();

            var exception = compositeArgumentException.InnerExceptions.Skip(exceptionNumber - 1).First();
            var propertyInfo = exception.GetType().GetProperty(property);
            propertyInfo.GetValue(exception).ToString().ShouldEqual(value);
        }
    }
}
