using System;
using CertificateRepository.ComponentModel;
using CertificateRepository.Validators.Core;

namespace CertificateRepository.Validators {
    public class ThumbprintValidator : IValidator<string> {
        private readonly IValidator<string> _beginsWithLeftToRightCharacterValdiator = new ThumbprintBeginsWithLeftToRightCharacterValidator();
        private readonly IValidator<string> _endsWithLeftToRightCharacterValdiator = new ThumbprintEndsWithLeftToRightCharacterValidator();
        private readonly IValidator<string> _stringValidator = new ThumbprintStringValidator(new ThumbprintCharacterValidator(), new ThumbprintStringLengthValidator());

        public ThumbprintValidator(IValidator<string> beginsWithLeftToRightCharacterValdiator, IValidator<string> endsWithLeftToRightCharacterValdiator, IValidator<string> stringValidator) {
            if (beginsWithLeftToRightCharacterValdiator == null) {
                throw new ArgumentNullException("beginsWithLeftToRightCharacterValdiator");
            }

            if (endsWithLeftToRightCharacterValdiator == null) {
                throw new ArgumentNullException("endsWithLeftToRightCharacterValdiator");
            }

            if (stringValidator == null) {
                throw new ArgumentNullException("stringValidator");
            }

            _beginsWithLeftToRightCharacterValdiator = beginsWithLeftToRightCharacterValdiator;
            _endsWithLeftToRightCharacterValdiator = endsWithLeftToRightCharacterValdiator;
            _stringValidator = stringValidator;
        }

        public ValidationResult IsValid(string thumbprint) {
            return Validate()
                .IsValid(thumbprint);
        }

        private IValidator<string> Validate() {
            return _beginsWithLeftToRightCharacterValdiator
                .And(_endsWithLeftToRightCharacterValdiator)
                .And(_stringValidator);
        }
    }
}
