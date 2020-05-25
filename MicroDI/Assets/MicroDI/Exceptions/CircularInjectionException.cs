using System;
using System.Runtime.Serialization;

namespace MicroDI.Exceptions
{
    public class CircularInjectionException : Exception
    {
        public CircularInjectionException()
        {
        }

        public CircularInjectionException(string message) : base(message)
        {
        }

        public CircularInjectionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CircularInjectionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
