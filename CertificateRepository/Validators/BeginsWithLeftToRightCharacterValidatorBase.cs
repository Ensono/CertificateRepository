using CertificateRepository.Validators.Core;

namespace CertificateRepository.Validators {
    public abstract class BeginsWithLeftToRightCharacterValidatorBase {
        private const string Message = "{0} begins with Right-To-Left-Mark (U+200E), this is often the result of " +
                                       "copying a input out of the Windows Certificate Manager.";

        private readonly string _context;
        private readonly string _paramName;

        public BeginsWithLeftToRightCharacterValidatorBase(string paramName, string context) {
            _paramName = paramName;
            _context = context;
        }

        public ValidationResult IsValid(string input) {
            if (input[0] == '\u200E') {
                return ValidationResult.Failure(string.Format(Message, _context), _paramName);
            }

            return ValidationResult.Success();
        }
    }
}