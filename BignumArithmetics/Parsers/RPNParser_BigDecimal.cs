using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BignumArithmetics.Parsers
{
    public class BigDecimalRPNParser : RPNParser<BigDecimal>
    {
        #region Variables
        private static Regex regex;
        #endregion

        #region Constructors
        static BigDecimalRPNParser()
        {
            funcs.Add("abs");
            regex = new Regex(String.Format(regexFormat, @"\d+(\.\d+)?|abs"), RegexOptions.Compiled);
        }
        #endregion

        #region Private
        protected override BigDecimal Number(string str)
        {
            return new BigDecimal(str);
        }
        protected override Queue<string> Tokenize(string str)
        {
            var stringTokens = new Queue<string>();
            int lastMatchPos = 0;
            int lastMatchLen = 0;
            Match match = regex.Match(str);
            while (match.Success)
            {
                lastMatchPos = match.Index;
                lastMatchLen = match.Value.Length;
                stringTokens.Enqueue(match.Value.Trim());
                match = match.NextMatch();
            }
            if (lastMatchPos + lastMatchLen < str.Length)
                throw new ArgumentException("Cannot calculate invalid expression");
            return stringTokens;
        }

        protected override BigDecimal CalcFunc(BigDecimal arg, string func)
        {
            switch (func)
            {
                case "abs":
                    arg = BigDecimal.Abs(arg);
                    break;
            }
            return arg;
        }
        #endregion
    }
}
