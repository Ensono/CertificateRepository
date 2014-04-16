using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificateRepository.Exceptions {
    [Serializable]
    public abstract class CompositeException<TException> : Exception where TException : Exception {
        public IEnumerable<TException> InnerExceptions { get; protected set; }
        
        public override string ToString() {
            var builder = new StringBuilder();
            var counter = 1;

            builder.AppendLine(base.ToString());

            foreach (var exception in InnerExceptions) {
                builder.AppendLine("  ------ Inner Exception #" + counter++ + " ------");
                builder.AppendLine(exception.ToString());
            }

            return builder.ToString();
        }
    }
}
