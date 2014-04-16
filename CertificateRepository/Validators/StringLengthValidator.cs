using System.Collections.Generic;
using CertificateRepository.ComponentModel;
using CertificateRepository.Validators.Core;

namespace CertificateRepository.Validators {
    public class StringLengthValidator : IValidator<string> {
        private const string Message = "Length of input string does not match expected. Expected: {0}. Actual: {1}.";
        
        private readonly int _length;

        public StringLengthValidator(int length) {
            _length = length;
        }

        public virtual ValidationResult IsValid(string input) {
            var result = new ValidationResult {
                ErrorMessages = new List<ValidationMessage>(),
                Valid = input.Length == _length
            };

            if (input.Length != _length) {
                result.ErrorMessages.Add(new ValidationMessage {
                    Message = string.Format(Message, _length, input.Length),
                    ParamName = "input"
                });
            }

            return result;
        }
    }
}