using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BignumArithmetics
{
    public struct RPNToken
    {
        public RPNParser<BigNumber>.TokenType tokenType;
        public string token;
    }
    public abstract class RPNParser<T> where T : BigNumber
    {
        public RPNParser(string str)
        {
            StringExpression = str;
        }
        public enum TokenType { OBracket, CBracket, Operation, Number }
        protected abstract T Number(RPNToken token);
        protected abstract Stack<RPNToken> Tokenize(string str);
        private Queue<RPNToken> ConvertToRPN(Stack<RPNToken> input)
        {
            throw new NotImplementedException();
        }
        private T CalculateRPNExpression(Queue<RPNToken> expression)
        {
            throw new NotImplementedException();
        }
        public T Parse()
        {
            Stack<RPNToken> tokens = Tokenize(StringExpression);
            Queue<RPNToken> tokens_RPN = ConvertToRPN(tokens);
            T answer = CalculateRPNExpression(tokens_RPN);
            return answer;
        }
        public string StringExpression { get; private set; }
    }
}
