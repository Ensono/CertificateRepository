using System;
using System.Linq;

namespace CertificateRepository.Exceptions {
    [Serializable]
    public class UnableToParseArgumentException : ArgumentException {
        private const string ErrorMessage = "Could not parse '{0}' as Type {1} for the '{2}' parameter.";

        private const string DetailedErrorMessage = "Could not parse '{0}' as Type {1} it must be one of the following values: {2}";

        private static Exception GetInnerException(string value, Type targetEnum) {
            var possibleValues = Enum.GetNames(targetEnum);

            var formattedValues = String.Join(String.Empty, possibleValues.Select(x => string.Format("{0} - {1}", Environment.NewLine, x)));
            
            return new FormatException(string.Format(DetailedErrorMessage, value, targetEnum.Name, formattedValues));
        }

        public UnableToParseArgumentException(string parameter, string value, Type targetEnum)
            : base(string.Format(ErrorMessage, value, targetEnum.Name, parameter), parameter, GetInnerException(value, targetEnum)) { }
    }
}
