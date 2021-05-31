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
                KeyExceptionType.Reserve =>  $"Key '{shortCode}' is a part of reserve keyword list and not allowed to be used in shortCode.",
                _ => throw new ArgumentOutOfRangeException(nameof(keyExceptionType), keyExceptionType, null)
            };
        }
    }
}
