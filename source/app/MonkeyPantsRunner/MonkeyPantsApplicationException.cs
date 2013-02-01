using System;
using System.Runtime.Serialization;

namespace MonkeyPants
{
    public class MonkeyPantsApplicationException : ApplicationException
    {
        public MonkeyPantsApplicationException()
        {
        }

        public MonkeyPantsApplicationException(string message) : base(message)
        {
        }

        public MonkeyPantsApplicationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MonkeyPantsApplicationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
