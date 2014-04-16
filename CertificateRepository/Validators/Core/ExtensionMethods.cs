using System.Collections.Generic;
using CertificateRepository.ComponentModel;

namespace CertificateRepository.Validators.Core {
    public static class ExtensionMethods {
        public static IValidator<TEntity> And<TEntity>(this IValidator<TEntity> validator1, IValidator<TEntity> validator2) {
            return new AndValidator<TEntity>(validator1, validator2);
        }

        public static IValidator<TEntity> Or<TEntity>(this IValidator<TEntity> validator1, IValidator<TEntity> validator2) {
            return new OrValidator<TEntity>(validator1, validator2);
        }

        public static IValidator<TEntity> Not<TEntity>(this IValidator<TEntity> validator) {
            return new NotValidator<TEntity>(validator);
        }

        public static void AddRange<T>(this IList<T> list, IEnumerable<T> items) {
            foreach (var item in items) {
                list.Add(item);
            }
        }
    }
}
