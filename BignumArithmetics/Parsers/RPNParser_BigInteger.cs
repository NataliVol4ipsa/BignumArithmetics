using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BignumArithmetics.Parsers
{
    public class BigIntegerRPNParser : RPNParser<BigInteger>
    {
        static BigIntegerRPNParser()
        {
            regex = new Regex(String.Format(regexFormat, @"\d+"), RegexOptions.Compiled);
        }
        public BigIntegerRPNParser(string str) : base(str) { }

        protected override BigInteger Number(string str)
        {
            return new BigInteger(str);
        }
        protected override Queue<string> Tokenize()
        {
            var stringTokens = new Queue<string>();
            int lastMatchPos = 0;
            int lastMatchLen = 0;
            Match match = regex.Match(StringExpression);
            while (match.Success)
            {
                lastMatchPos = match.Index;
                lastMatchLen = match.Value.Length;
                stringTokens.Enqueue(match.Value.Trim());
                match = match.NextMatch();
            }
            if (lastMatchPos + lastMatchLen < StringExpression.Length)
                throw new ArgumentException("Cannot calculate invalid expression");
            return stringTokens;
        }

        private static Regex regex;
    }
}
