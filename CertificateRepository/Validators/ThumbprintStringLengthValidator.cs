using CertificateRepository.Validators.Core;

namespace CertificateRepository.Validators {
    public class ThumbprintStringLengthValidator : StringLengthValidator {
        public ThumbprintStringLengthValidator()
            : base(40) { }

        public override ValidationResult IsValid(string input) {
            var result = base.IsValid(input);

            foreach (var message in result.ErrorMessages) {
                message.ParamName = "thumbprint";
            }

            return result;
        }
    }
}