using System;

namespace ShortLink.Exceptions
{
    public class InvalidRequestException : Exception
    {
        public string ParameterName { get; }

        public InvalidRequestException(string parameterName)
        {
            ParameterName = parameterName;
        }
    }
}
