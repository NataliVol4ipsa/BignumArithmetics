using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BignumArithmetics.Parsers
{
    public class RPNParser : RPNParser<BigInteger>
    {
        static RPNParser()
        {
            regex = new Regex(String.Format(regexFormat, @"\d+"), RegexOptions.Compiled);
        }
        public RPNParser(string str) : base(str) { }

        protected override BigInteger Number(string str)
        {
            return new BigInteger(str);
        }

        protected override Queue<RPNToken> Tokenize()
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
            return RecognizeLexemS(stringTokens);
        }
        //move this to main parser syntax analyze! take strings,  calc
        //takes valid strings
        private Queue<RPNToken> RecognizeLexemS(Queue<string> stringTokens)
        {
            var tokenQueue = new Queue<RPNToken>();
            tokenQueue.Enqueue(new RPNToken("", TokenType.Spigot));
            TokenType tokenType;
            foreach(var token in stringTokens)
            {
                switch(token)
                {
                    case "+":
                        tokenType = TokenType.Operation_priority1;
                        break;
                    case "-":
                        tokenType = TokenType.Operation_priority1;
                        break;
                    case "*":
                        tokenType = TokenType.Operation_priority2;
                        break;
                    case "/":
                        tokenType = TokenType.Operation_priority2;
                        break;
                    case "%":
                        tokenType = TokenType.Operation_priority2;
                        break;
                    case "(":
                        tokenType = TokenType.OBracket;
                        break;
                    case ")":
                        tokenType = TokenType.CBracket;
                        break;
                    default:
                        tokenType = TokenType.Number;
                        break;
                }
                tokenQueue.Enqueue(new RPNToken(token, tokenType));
            }
            tokenQueue.Enqueue(new RPNToken("", TokenType.Spigot));
            return tokenQueue;
        }


        private static Regex regex;
    }
}
