using System;
using System.Linq;
using CertificateRepository.ComponentModel;
using CertificateRepository.Validators.Core;

namespace CertificateRepository.Validators {
    public class ThumbprintCharacterValidator : IValidator<char> {
        private readonly char[] _validCharacters = "0123456789AaBbCcDdEeFf".ToCharArray();
        private const string Message = "The character '\\u{0:X4}' is not valid in this context.";

        public ValidationResult IsValid(char input) {
            if (_validCharacters.Contains(input)) {
                return ValidationResult.Success();
            }

            return ValidationResult.Failure(String.Format(Message, (int)input), "input");
        }
    }
}
