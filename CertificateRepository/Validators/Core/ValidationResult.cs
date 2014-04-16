using System.Collections.Generic;

namespace CertificateRepository.Validators.Core {
    public class ValidationResult {
        public bool Valid { get; set; }
        public IList<ValidationMessage> ErrorMessages { get; set; }

        public ValidationResult() {
            ErrorMessages = new List<ValidationMessage>();
        }

        public static ValidationResult operator &(ValidationResult lhs, ValidationResult rhs) {
            var result = new ValidationResult {
                Valid = lhs.Valid & rhs.Valid,
                ErrorMessages = new List<ValidationMessage>()
            };

            result.ErrorMessages.AddRange(lhs.ErrorMessages);
            result.ErrorMessages.AddRange(rhs.ErrorMessages);

            return result;
        }

        public static ValidationResult operator |(ValidationResult lhs, ValidationResult rhs) {
            var result = new ValidationResult {
                Valid = lhs.Valid | rhs.Valid,
                ErrorMessages = new List<ValidationMessage>()
            };

            result.ErrorMessages.AddRange(lhs.ErrorMessages);
            result.ErrorMessages.AddRange(rhs.ErrorMessages);

            return result;
        }

        public static ValidationResult operator !(ValidationResult validationResult) {
            validationResult.Valid = !validationResult.Valid;

            return validationResult;
        }

        public static ValidationResult Success() {
            return new ValidationResult {
                Valid = true,
                ErrorMessages = new List<ValidationMessage>()
            };
        }

        public static ValidationResult Failure(string message, string argument) {
            return new ValidationResult {
                Valid = false,
                ErrorMessages = new List<ValidationMessage> {
                    new ValidationMessage { Message = message, ParamName = argument}
                }
            };
        }
    }
}
