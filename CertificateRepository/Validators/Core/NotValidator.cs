using System;
using CertificateRepository.ComponentModel;

namespace CertificateRepository.Validators.Core {
    internal class NotValidator<TEntity> : IValidator<TEntity> {
        private readonly IValidator<TEntity> _wrapped;

        protected IValidator<TEntity> Wrapped {
            get {
                return _wrapped;
            }
        }

        internal NotValidator(IValidator<TEntity> validator) {
            if (validator == null) {
                throw new ArgumentNullException("validator");
            }

            _wrapped = validator;
        }

        public ValidationResult IsValid(TEntity entity) {
            return !Wrapped.IsValid(entity);
        }
    }
}
