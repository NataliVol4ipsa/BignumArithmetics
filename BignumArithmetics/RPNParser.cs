using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BignumArithmetics
{
    public enum TokenType
    {
        Spigot, //at the beginning and the end of expression
        OBracket, // (
        CBracket, // )
        Operation_priority1, // +, -
        Operation_priority2, // *, /, %
        Number // T
    }

    public struct RPNToken
    {
        public TokenType tokenType;
        public string str;
    }

    public abstract class RPNParser<T> where T : BigNumber
    {
        static RPNParser()
        {
            actions = new rpnAction[5, 6]
            {
                {RmBrackets,  InputToBuf,  InputToBuf,  InputToBuf,  InputToBuf, Error       },
                {BufToResult, BufToResult, BufToResult, InputToBuf,  InputToBuf, BufToResult },
                {BufToResult, BufToResult, BufToResult, InputToBuf,  InputToBuf, BufToResult },
                {BufToResult, BufToResult, BufToResult, BufToResult, InputToBuf, BufToResult },
                {Error,       InputToBuf,  InputToBuf,  InputToBuf,  InputToBuf, RmBrackets  }
            };
        }
        delegate void rpnAction(Stack<RPNToken> input, Stack<RPNToken> buffer, Queue<RPNToken> result);
       

        public RPNParser(string str)
        {
            StringExpression = str;
        }
       
        protected abstract T Number(string str);
        protected abstract Stack<RPNToken> Tokenize(string str);
        private Queue<RPNToken> ConvertToRPN(Stack<RPNToken> input)
        {
            Queue<RPNToken> result = new Queue<RPNToken>();
            Stack<RPNToken> buffer = new Stack<RPNToken>();
            InputToBuf(input, buffer, result);
            while (input.Count > 0)
                actions[(int)buffer.Peek().tokenType, (int)input.Peek().tokenType](input,buffer,result);
            return result;
        }
        private T CalculateRPNExpression(Queue<RPNToken> expression)
        {
            //todo: if empty?
            RPNToken current;
            Stack<T> calcBuf = new Stack<T>();
            while (expression.Count > 0)
            {
                current = expression.Dequeue();
                if (current.tokenType == TokenType.Number)
                    calcBuf.Push(Number(current.str));
                else
                    try
                    {
                        calcBuf.Push(DoOp(calcBuf.Pop(), calcBuf.Pop(), current.str));
                    }
                    catch
                    {
                        throw new ArgumentException("Cannot calculate this expression");
                    }
            }
            if (calcBuf.Count > 1)
                throw new ArgumentException("Cannot calculate this expression");
            return calcBuf.Pop();
        }
        private T DoOp(T right, T left, string op)
        {
            if (op.Equals("+"))
                return (T)(left + right);
            if (op.Equals("-"))
                return (T)(left - right);
            if (op.Equals("*"))
                return (T)(left * right);
            if (op.Equals("/"))
                return (T)(left / right);
            if (op.Equals("%"))
                return (T)(left % right);
            throw new NotImplementedException("RPNParser DoOp met unimplemented operator");
        }
        public T Parse()
        {
            Stack<RPNToken> tokens = Tokenize(StringExpression);
            Queue<RPNToken> tokens_RPN = ConvertToRPN(tokens);
            T answer = CalculateRPNExpression(tokens_RPN);
            return answer;
        }
        public string StringExpression { get; private set; }

        #region Actions
        static rpnAction[,] actions;

        static rpnAction InputToBuf = (input, buffer, result) => buffer.Push(input.Pop());
        static rpnAction BufToResult = (input, buffer, result) => result.Enqueue(buffer.Pop());
        static rpnAction RmBrackets = (input, buffer, result) => { buffer.Pop(); input.Pop(); };
        static rpnAction Error = (input, buffer, result) => { throw new ArgumentException("Cannot calculate invalid expression"); };
        #endregion
    }
}
