using System;
using ShortLink.Enums;

namespace ShortLink.Exceptions
{
    public class KeyException : Exception
    {
        public KeyException(string shortCode, KeyExceptionType type)
        : base(GetMessage(shortCode, type))
        {
        }

        private static string GetMessage(string shortCode, KeyExceptionType keyExceptionType)
        {
            return keyExceptionType switch
            {
                KeyExceptionType.Duplicate => $"Key '{shortCode}' already exist in storage.",
                KeyExceptionType.NotFound => $"Key '{shortCode}' does not exist in storage.",
                _ => throw new ArgumentOutOfRangeException(nameof(keyExceptionType), keyExceptionType, null)
            };
        }
    }
}
