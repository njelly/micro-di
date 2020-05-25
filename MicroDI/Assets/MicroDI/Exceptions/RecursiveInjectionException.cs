using System;
using System.Runtime.Serialization;

namespace MicroDI.Exceptions
{
    public class RecursiveInjectionException : Exception
    {
        public RecursiveInjectionException()
        {
        }

        public RecursiveInjectionException(string message) : base(message)
        {
        }

        public RecursiveInjectionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RecursiveInjectionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
