using System;
using System.Collections.Generic;

//todo: add minus and unar operations support

namespace BignumArithmetics.Parsers
{
    public enum TokenType
    {
        Spigot, //at the beginning and the end of expression
        Operation_priority1, // +, -
        Operation_priority2, // *, /, %
        OBracket, // (
        CBracket, // )
        Number // T
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
        #region Constructors
        static RPNParser()
        {
            actions = new rpnAction[4, 5]
            {
                {RmBrackets,  InputToBuf,  InputToBuf,  InputToBuf, Error      },
                {BufToResult, BufToResult, InputToBuf,  InputToBuf, BufToResult},
                {BufToResult, BufToResult, BufToResult, InputToBuf, BufToResult},
                {Error,       InputToBuf,  InputToBuf,  InputToBuf, RmBrackets }
            };
        }       
        public RPNParser(string str)
        {
            StringExpression = str.Trim();
        }
        #endregion

        #region Abstract methods
        protected abstract T Number(string str);
        protected abstract Queue<string> Tokenize();
        #endregion

        #region Protected Algorithm methods 
        protected Queue<RPNToken> RecognizeLexems(Queue<string> stringTokens)
        {
            var tokenQueue = new Queue<RPNToken>();
            tokenQueue.Enqueue(new RPNToken("", TokenType.Spigot));
            TokenType tokenType;
            foreach (var token in stringTokens)
            {
                switch (token)
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
        protected Queue<RPNToken> AnalyseSyntax (Queue<RPNToken> tokens)
        {
            var cleanTokens = new Queue<RPNToken>();
            return tokens;
            //todo: FINISH THIS
            //check operations binarity
            //check multiple signs in a row
            throw new NotImplementedException();
            return cleanTokens;
        }
        protected Queue<RPNToken> ConvertToRPN(Queue<RPNToken> input)
        {
            var result = new Queue<RPNToken>();
            var buffer = new Stack<RPNToken>();
            InputToBuf(input, buffer, result);
            while (input.Count > 0)
            {
                if (input.Peek().tokenType == TokenType.Number)
                    InputToResult(input, buffer, result);
                else
                    actions[(int)buffer.Peek().tokenType, (int)input.Peek().tokenType](input, buffer, result);
            }
            return result;
        }
        protected T CalculateRPNExpression(Queue<RPNToken> expression)
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
                throw new ArgumentException("Cannot calculate this expression. Buf is not empty");
            return calcBuf.Pop();
        }
        protected T DoOp(T right, T left, string op)
        {
            if (op.Equals("+"))
                return (T)(left + right);
            if (op.Equals("*"))
                return (T)(left * right);
            if (op.Equals("/"))
                return (T)(left / right);
            if (op.Equals("%"))
                return (T)(left % right);
            throw new NotImplementedException("RPNParser met unimplemented operator");
        }
        #endregion

        #region Public Parse methods
        //todo: two parsing funcs : for postfix and infix string? constructor bool isInfix?
        //this is in case of AnalyzeSyntax
        public T Parse()
        {
            Queue<string> stringTokens = Tokenize();
            Queue<RPNToken> tokens = RecognizeLexems(stringTokens);
            Queue<RPNToken> cleanTokens = AnalyseSyntax(tokens);
            Queue<RPNToken> tokens_RPN = ConvertToRPN(cleanTokens);
            T answer = CalculateRPNExpression(tokens_RPN);
            return answer;
        }
        #endregion

        #region Variables
        public string StringExpression { get; private set; }
        protected static string regexFormat = @"\G\s*({0}|\+|-|\*|/|%|\(|\))\s*";
        #endregion

        #region Actions
        private delegate void rpnAction(Queue<RPNToken> input, Stack<RPNToken> buffer, Queue<RPNToken> result);
        private static readonly rpnAction[,] actions;

        private static readonly rpnAction InputToResult =
            (input, buffer, result) => result.Enqueue(input.Dequeue());
        private static readonly rpnAction InputToBuf = 
            (input, buffer, result) => buffer.Push(input.Dequeue());
        private static readonly rpnAction BufToResult = 
            (input, buffer, result) => result.Enqueue(buffer.Pop());
        private static readonly rpnAction RmBrackets = 
            (input, buffer, result) => { buffer.Pop(); input.Dequeue(); };
        private static readonly rpnAction Error = 
            (input, buffer, result) => throw new ArgumentException("Cannot convert invalid expression");
        #endregion
    }
}
