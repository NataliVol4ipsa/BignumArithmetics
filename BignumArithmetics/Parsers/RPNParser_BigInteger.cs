using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BignumArithmetics.Parsers
{
    public class BigIntegerRPNParser : RPNParser<BigInteger>
    {
        private static Regex regex;

        static BigIntegerRPNParser()
        {
            funcs.Add("abs");
            regex = new Regex(String.Format(regexFormat, @"\d+|abs"), RegexOptions.Compiled);
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

        protected override BigInteger CalcFunc(BigInteger arg, string func)
        {
            switch(func)
            {
                case "abs":
                    arg = BigInteger.Abs(arg);
                    break;
            }
            return arg;
        }
    }
}
