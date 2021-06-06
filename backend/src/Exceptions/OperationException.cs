using System;
using ShortLink.Enums;

namespace ShortLink.Exceptions
{
    public class OperationException : Exception
    {
        public Operation Operation { get; }
        

        public Exception OriginalException { get; }

        public OperationException(Operation operation)
        {
            Operation = operation;
        }

        public OperationException(Operation operation, Exception originalException)
        {
            Operation = operation;
            OriginalException = originalException;
        }

        public OperationException(Operation operation, string? message) : base (message)
        {
            Operation = operation;
        }
    }
}
