using CertificateRepository.ComponentModel;

namespace CertificateRepository.Validators {
    public class ThumbprintBeginsWithLeftToRightCharacterValidator : BeginsWithLeftToRightCharacterValidatorBase, IValidator<string> {
        public ThumbprintBeginsWithLeftToRightCharacterValidator()
            : base("thumbprint", "Thumbprint") {}
    }
}
