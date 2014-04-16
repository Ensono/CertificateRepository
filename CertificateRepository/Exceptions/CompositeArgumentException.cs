using System;
using System.Collections.Generic;
using System.Linq;
using CertificateRepository.Validators.Core;

namespace CertificateRepository.Exceptions {
    public class CompositeArgumentException : CompositeException<ArgumentException> {
        public CompositeArgumentException(IEnumerable<ValidationMessage> validationMessages) {
            if (validationMessages == null) {
                throw new ArgumentNullException("validationMessages");
            }

            var messages = validationMessages as IList<ValidationMessage> ?? validationMessages.ToList();
            if (!messages.Any()) {
                throw new ArgumentException("Can not be empty.", "validationMessages");
            }

            InnerExceptions = messages.Select(x => new ArgumentException(x.Message, x.ParamName));
        }
    }
}