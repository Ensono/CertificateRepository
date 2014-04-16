using CertificateRepository.ComponentModel;
using CertificateRepository.Validators.Core;

namespace CertificateRepository.Validators {
    public class ThumbprintEndsWithLeftToRightCharacterValidator : IValidator<string> {
        private const string Message = "Thumprint ends with Right-To-Left-Mark (U+200E), this is often the result of " +
                                       "copying a thumbprint out of the Windows Certificate Manager.";
            
        public ValidationResult IsValid(string thumbprint) {
            if (thumbprint[thumbprint.Length - 1] == '\u200E') {
                return ValidationResult.Failure(Message, "thumbprint");
            }

            return ValidationResult.Success();
        }
    }
}