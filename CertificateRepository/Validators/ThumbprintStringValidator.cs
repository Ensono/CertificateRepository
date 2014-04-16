using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using CertificateRepository.ComponentModel;
using CertificateRepository.Validators.Core;

namespace CertificateRepository.Validators {
    public class ThumbprintStringValidator : IValidator<string> {
        private readonly IValidator<char> _thumbprintCharacterValidator;
        private readonly IValidator<string> _lengthValidator;

        public ThumbprintStringValidator(IValidator<char> thumbprintCharacterValidator, IValidator<string> lengthValidator) {
            if (thumbprintCharacterValidator == null) {
                throw new ArgumentNullException("thumbprintCharacterValidator");
            }

            if (lengthValidator == null) {
                throw new ArgumentNullException("lengthValidator");
            }

            _thumbprintCharacterValidator = thumbprintCharacterValidator;
            _lengthValidator = lengthValidator;
        }

        public ValidationResult IsValid(string thumbprint) {
            var validationResult = _lengthValidator.IsValid(thumbprint);
            var counter = 0;

            foreach (var character in thumbprint) {
                int position = counter++;
                var characterResult = _thumbprintCharacterValidator.IsValid(character);

                validationResult.Valid = validationResult.Valid && characterResult.Valid;
                Func<ValidationMessage, ValidationMessage> selector = 
                    x =>
                        new ValidationMessage {
                            Message = string.Format("Invalid character at position {0}: {1}", position, x.Message),
                            ParamName = "thumbprint"
                        };

                validationResult.ErrorMessages.AddRange(characterResult.ErrorMessages.Select(selector));
            }

            return validationResult;
        }
    }
}