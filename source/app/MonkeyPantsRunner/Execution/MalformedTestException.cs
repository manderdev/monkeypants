using System;

namespace MonkeyPants.Execution
{
    public class MalformedTestException : Exception
    {
        public MalformedTestException(string message) : base(message)
        {
        }

        public MalformedTestException(string message, Exception ex) : base(message, ex)
        {           
        }
    }
}