using System;
using System.Runtime.Serialization;

namespace PictlHelpers.Exceptions
{
    [Serializable]
    public class WrongPasswordException : Exception
    {
        const string DEFAULT_MESSAGE = "Wrong password!";
        public WrongPasswordException() : base(DEFAULT_MESSAGE) { }
        public WrongPasswordException(string message) : base(message) { }
        public WrongPasswordException(string message, Exception inner) : base(message, inner) { }
        protected WrongPasswordException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    }
}
