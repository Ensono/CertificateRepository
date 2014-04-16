using System;
using CertificateRepository.ComponentModel;

namespace CertificateRepository.Validators.Core {
    internal class OrValidator<TEntity> : IValidator<TEntity> {
        private readonly IValidator<TEntity> _validator1;
        private readonly IValidator<TEntity> _validator2;

        protected IValidator<TEntity> Validator1 {
            get {
                return _validator1; 
            }
        }

        protected IValidator<TEntity> Validator2 {
            get {
                return _validator2;
            }
        }

        internal OrValidator(IValidator<TEntity> validator1, IValidator<TEntity> validator2) {
            if (validator1 == null) {
                throw new ArgumentNullException("validator1");
            }

            if (validator2 == null) {
                throw new ArgumentNullException("validator2");
            }

            _validator1 = validator1;
            _validator2 = validator2;
        }

        public ValidationResult IsValid(TEntity thumbprint) {
            return Validator1.IsValid(thumbprint) | Validator2.IsValid(thumbprint);
        }
    }
}
