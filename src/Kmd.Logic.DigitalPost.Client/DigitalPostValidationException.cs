using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Kmd.Logic.DigitalPost.Client
{
    [Serializable]
    public class DigitalPostValidationException : Exception
    {
        public IDictionary<string, IList<string>> ValidationErrors { get; }

        public DigitalPostValidationException()
        {
        }

        public DigitalPostValidationException(IDictionary<string, IList<string>> validationErrors)
            : base(GenerateMessage(validationErrors))
        {
            this.ValidationErrors = validationErrors;
        }

        public DigitalPostValidationException(string message, IDictionary<string, IList<string>> validationErrors)
            : base(message)
        {
            this.ValidationErrors = validationErrors;
        }

        public DigitalPostValidationException(string message, IDictionary<string, IList<string>> validationErrors, Exception innerException)
            : base(message, innerException)
        {
            this.ValidationErrors = validationErrors;
        }

        public DigitalPostValidationException(IDictionary<string, IList<string>> validationErrors, Exception innerException)
            : base(GenerateMessage(validationErrors), innerException)
        {
        }

        public DigitalPostValidationException(string message)
            : base(message)
        {
        }

        public DigitalPostValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected DigitalPostValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        private static string GenerateMessage(IDictionary<string, IList<string>> validationErrors)
        {
            var message = "Invalid digital post parameters";

            if (validationErrors != null && validationErrors.Count > 0)
            {
                message += " (" + string.Join(";", validationErrors.Select(x => $"{x.Key}: {string.Join(",", x.Value)}")) + ")";
            }

            return message;
        }
    }
}