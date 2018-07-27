using System;

namespace BignumArithmetics.Parsers
{
    public class RPNParserException : Exception
    {
        public RPNParserException() { }
        public RPNParserException(string message)
            : base(message) { }
        public RPNParserException(string message, Exception inner)
            : base(message, inner) { }
    }
}
