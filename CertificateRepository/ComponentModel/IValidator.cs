using CertificateRepository.Validators.Core;

namespace CertificateRepository.ComponentModel {
    public interface IValidator<in TEntity> {
        ValidationResult IsValid(TEntity thumbprint);
    }
}
