using System;
using System.Runtime.Serialization;

namespace Kmd.Logic.DigitalPost.Client
{
    [Serializable]
    public class DigitalPostConfigurationException : Exception
    {
        public string InnerMessage { get; }

        public DigitalPostConfigurationException()
        {
        }

        public DigitalPostConfigurationException(string message)
            : base(message)
        {
        }

        public DigitalPostConfigurationException(string message, string innerMessage)
            : base(message)
        {
            this.InnerMessage = innerMessage;
        }

        public DigitalPostConfigurationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected DigitalPostConfigurationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}