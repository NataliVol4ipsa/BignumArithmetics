using System;
using System.Collections.Generic;

namespace BignumArithmetics.Parsers
{
    //todo: rearrange actions array and this enum
    public enum TokenType
    {
        Spigot, //at the beginning and the end of expression
        Operation_priority1, // +, -
        Number, // T
        Operation_priority2, // *, /, %
        OBracket, // (
        CBracket // )
    }

    public struct RPNToken
    {
        public RPNToken(string str, TokenType tokenType)
        {
            this.str = str;
            this.tokenType = tokenType;
        }
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
        delegate void rpnAction(Queue<RPNToken> input, Stack<RPNToken> buffer, Queue<RPNToken> result);
       
        public RPNParser(string str)
        {
            StringExpression = str.Trim();
        }
       
        protected abstract T Number(string str);
        protected abstract Queue<RPNToken> Tokenize();

        private Queue<RPNToken> ConvertToRPN(Queue<RPNToken> input)
        {
            var result = new Queue<RPNToken>();
            var buffer = new Stack<RPNToken>();
            InputToBuf(input, buffer, result);
            while (input.Count > 0)
                actions[(int)buffer.Peek().tokenType, (int)input.Peek().tokenType](input,buffer,result);
            return result;
        }
        private T CalculateRPNExpression(Queue<RPNToken> expression)
        {
            //todo: if empty?
            RPNToken current;
            var calcBuf = new Stack<T>();
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
        //todo: is this fine?
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
            throw new NotImplementedException("RPNParser met unimplemented operator");
        }

        public T Parse()
        {
            Queue<RPNToken> tokens = Tokenize();
            //todo: validade mess between numbers such as many signs.
            Queue<RPNToken> tokens_RPN = ConvertToRPN(tokens);
            T answer = CalculateRPNExpression(tokens_RPN);
            return answer;
        }

        public string StringExpression { get; private set; }
        protected static string regexFormat = @"\G\s*({0}|\+|-|\*|\\|%|\(|\))\s*";

        #region Actions
        //todo: make buffers local vars and lambdas take no parameters
        static rpnAction[,] actions;

        static rpnAction InputToBuf = (input, buffer, result) => buffer.Push(input.Dequeue());
        static rpnAction BufToResult = (input, buffer, result) => result.Enqueue(buffer.Pop());
        static rpnAction RmBrackets = (input, buffer, result) => { buffer.Pop(); input.Dequeue(); };
        static rpnAction Error = (input, buffer, result) => { throw new ArgumentException("Cannot calculate invalid expression"); };
        #endregion
    }
}
