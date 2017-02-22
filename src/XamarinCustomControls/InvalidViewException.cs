using System;

namespace Xamarin.CustomControls
{
    public class InvalidViewException : Exception
    {
        public InvalidViewException()
        {
        }

        public InvalidViewException(string message) : base(message)
        {
        }

        public InvalidViewException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}