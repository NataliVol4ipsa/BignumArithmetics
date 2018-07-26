using System;
using System.Linq;
using System.Collections.Generic;

//todo: add minus and unar operations support
//todo: split code into files
//todo: test abs() func

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
    public enum TokenType
    {
        Empty,
        BinOp,
        UnOp,
        OBracket,
        CBracket,
        Number,
        Function
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

    public enum OpArity { Unary, Binary }
    public enum OpAssoc { Left, Right }
    public struct OpInfo
    {
        public OpInfo(string op, OpArity arity, int priority, OpAssoc assoc)
        {
            this.op = op;
            this.arity = arity;
            this.priority = priority;
            this.assoc = assoc;
        }
        public string op;
        public OpArity arity;
        public int priority;
        public OpAssoc assoc;

    }

    public abstract class RPNParser<T> where T : BigNumber
    {
        #region Variables
        protected static readonly string regexFormat;
        protected static readonly ILookup<string, OpInfo> OpInfoMap;

        private Queue<RPNToken> expression = new Queue<RPNToken>();
        private Stack<RPNToken> buffer = new Stack<RPNToken>();
        private Stack<T> result = new Stack<T>();
        #endregion

        #region Properties
        public string StringExpression { get; private set; }
        #endregion

        #region Constructors
        static RPNParser()
        {
            regexFormat = @"\G\s*({0}|\+|-|\*|/|%|\(|\))\s*";
            OpInfoMap = new[]
            {
                new OpInfo("-", OpArity.Binary, 1, OpAssoc.Left),
                new OpInfo("-", OpArity.Unary,  3, OpAssoc.Left),
                new OpInfo("+", OpArity.Binary, 1, OpAssoc.Left),
                new OpInfo("+", OpArity.Unary,  3, OpAssoc.Left),
                new OpInfo("*", OpArity.Binary, 2, OpAssoc.Left),
                new OpInfo("/", OpArity.Binary, 2, OpAssoc.Left),
                new OpInfo("^", OpArity.Binary, 2, OpAssoc.Right)
            }.ToLookup(op => op.op);
        }

        public RPNParser(string str)
        {
            StringExpression = str.Trim();
        }
        #endregion

        //todo: two parsing funcs : for postfix and infix string? constructor bool isInfix?
        #region Public Parse methods
        public T Parse()
        {
            Queue<string> stringTokens = Tokenize();
            expression = RecognizeLexems(stringTokens);
            T answer;
            try
            {
                answer = CalculateExpression();// <<<<<<<
            }
            catch (InvalidOperationException)
            {
                throw new RPNParserException("Invalid expression format");
            }
            return answer;
        }
        #endregion

        #region Abstract methods
        protected abstract T Number(string str);
        protected abstract Queue<string> Tokenize();
        #endregion

        #region Protected Algorithm methods 
        //todo: check exception
        private T ToNumber(string str)
        {
            T ans;
            try
            {
                ans = Number(str);
            }
            catch
            {
                throw new RPNParserException("Invalid number \""
                    + str + "\" parsed by " + this.GetType());
            }
            return ans;
        }
        private OpInfo GetOpInfo(RPNToken token)
        {
            OpArity arity = token.tokenType == TokenType.BinOp ?
                                    OpArity.Binary :
                                    OpArity.Unary;
            var curOps = OpInfoMap[token.str];
            var curOp = curOps.Count() == 1 ?
                                    curOps.Single() :
                                    curOps.Single(o => o.arity == arity);
            return curOp;
        }
        private bool IsOp(RPNToken t)
        {
            if (t.tokenType == TokenType.BinOp || t.tokenType == TokenType.UnOp)
                return true;
            return false;
        }
        //returns 1 or -1. true||false?
        private int CmpPriorities(OpInfo left, OpInfo right)
        {
            if ((right.assoc == OpAssoc.Left  && right.priority <= left.priority) ||
                (right.assoc == OpAssoc.Right && right.priority < left.priority))
                return 1;
            return 0;
        }
        //todo: add funcs
        protected Queue<RPNToken> RecognizeLexems(Queue<string> stringTokens)
        {
            var tokenQueue = new Queue<RPNToken>();
            TokenType tokenType;
            foreach (var token in stringTokens)
            {
                if (OpInfoMap[token].Count() > 0)
                {
                    if (tokenQueue.Count() > 0 && (
                        tokenQueue.Peek().tokenType == TokenType.CBracket ||
                        tokenQueue.Peek().tokenType == TokenType.Number))
                        tokenType = TokenType.BinOp;
                    else
                        tokenType = TokenType.UnOp;
                }
                else if (token == "(")
                    tokenType = TokenType.OBracket;
                else if (token == ")")
                    tokenType = TokenType.CBracket;
                else
                    tokenType = TokenType.Number;
                tokenQueue.Enqueue(new RPNToken(token, tokenType));
            }
            return tokenQueue;
        }
        protected T CalculateExpression()
        {
            //todo: if empty?
            RPNToken currentToken;
            RPNToken prevToken = new RPNToken("", TokenType.Empty);
            RPNToken tempToken;
            OpInfo curOpInfo;
            while (expression.Count > 0)
            {
                currentToken = expression.Dequeue();
                if (currentToken.tokenType == TokenType.Number)
                    result.Push(ToNumber(currentToken.str));
                else if (currentToken.tokenType == TokenType.Function)
                    buffer.Push(currentToken);
                else if (currentToken.tokenType == TokenType.BinOp || 
                    currentToken.tokenType == TokenType.UnOp)
                {
                    curOpInfo = GetOpInfo(currentToken);
                    while (buffer.Count() > 0)
                    {
                        RPNToken cmpToken = buffer.Peek();
                        if (cmpToken.tokenType == TokenType.Function ||
                             (IsOp(cmpToken) && CmpPriorities(GetOpInfo(cmpToken), curOpInfo) < 0))
                            CalcToken(buffer.Pop());
                        else
                            break;

                    }
                    buffer.Push(currentToken);
                }
                else if (currentToken.tokenType == TokenType.OBracket)
                    buffer.Push(currentToken);
                else if (currentToken.tokenType == TokenType.CBracket)
                {
                    while ((tempToken = buffer.Pop()).tokenType != TokenType.CBracket)
                        CalcToken(tempToken);
                    if (buffer.Count() > 0 && buffer.Peek().tokenType == TokenType.Function)
                        CalcToken(buffer.Pop());
                }
                else
                    throw new NotImplementedException("Unimplemented token");
                prevToken = currentToken;
            }
            while (buffer.Count() > 0)
                CalcToken(buffer.Pop());
            if (result.Count() > 1)
                throw new ArgumentException("Cannot calculate this expression. Remaining buf is not empty");
            else if (result.Count() == 0)
                return ToNumber("0");
            return result.Pop();
        }
        protected void CalcToken(RPNToken op)
        {
            if (op.tokenType == TokenType.Function)
                result.Push(CalcFunc(result.Pop(), op.str));
            else if (op.tokenType == TokenType.BinOp)
                result.Push(CalcBinaryOp(result.Pop(), result.Pop(), op.str));
            else if (op.tokenType == TokenType.UnOp)
                result.Push(CalcUnaryOp(result.Pop(), op.str));
        }
        protected T CalcBinaryOp(T right, T left, string op)
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
        //todo: implement
        protected T CalcUnaryOp(T operand, string op)
        {
            return operand;
        }
        //todo: implement
        protected T CalcFunc(T arg, string func)
        {
            return arg;
        }
        #endregion


    }
}
