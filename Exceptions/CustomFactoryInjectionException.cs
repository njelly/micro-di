using System;
using System.Runtime.Serialization;

namespace MicroDI.Exceptions
{
    public class CustomFactoryInjectionException : Exception
    {
        public CustomFactoryInjectionException()
        {
        }

        public CustomFactoryInjectionException(string message) : base(message)
        {
        }

        public CustomFactoryInjectionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CustomFactoryInjectionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
