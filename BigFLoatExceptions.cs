using System;

namespace net.NataliVol4ica.BignumArithmetics
{
    public class NumberFormatException : Exception
    {
        public NumberFormatException() { }
        public NumberFormatException(string message)
            : base(message) { }
        public NumberFormatException(string message, Exception inner)
            : base(message, inner) { }
    }
}