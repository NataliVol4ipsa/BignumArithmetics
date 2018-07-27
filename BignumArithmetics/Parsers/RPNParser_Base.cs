using System;
using System.Linq;
using System.Collections.Generic;

//todo: split code into files ?

namespace BignumArithmetics.Parsers
{
    public abstract class RPNParser<T> where T : BigNumber
    {
        #region Enums
        protected enum TokenType
        {
            BinOp,
            UnOp,
            OBracket,
            CBracket,
            Number,
            Function
        }
        protected enum OpArity { Unary, Binary }
        protected enum OpAssoc { Left, Right }
        #endregion

        #region Structs
        protected struct RPNToken
        {
            public RPNToken(string str, TokenType tokenType)
            {
                this.str = str;
                this.tokenType = tokenType;
            }
            public TokenType tokenType;
            public string str;
        }
        protected struct OpInfo
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
        protected struct RPNBufs
        {
            public Queue<string> stringTokens;
            public Queue<RPNToken> expression;
            public Stack<RPNToken> buffer;
            public Stack<T> result;
        }
        #endregion

        #region Variables

        #region Static Const
        protected static readonly string regexFormat;
        #endregion

        #region Static
        protected static ILookup<string, OpInfo> opInfoMap;
        protected static List<string> funcs;
        #endregion

        #endregion
   
        #region Constructors
        static RPNParser()
        {
            regexFormat = @"\G\s*({0}|\+|-|\*|/|%|\(|\))\s*";
            opInfoMap = new[]
            {
                new OpInfo("-", OpArity.Binary, 1, OpAssoc.Left),
                new OpInfo("-", OpArity.Unary,  3, OpAssoc.Left),
                new OpInfo("+", OpArity.Binary, 1, OpAssoc.Left),
                new OpInfo("+", OpArity.Unary,  3, OpAssoc.Left),
                new OpInfo("*", OpArity.Binary, 2, OpAssoc.Left),
                new OpInfo("/", OpArity.Binary, 2, OpAssoc.Left),
                new OpInfo("%", OpArity.Binary, 2, OpAssoc.Left),
                new OpInfo("^", OpArity.Binary, 2, OpAssoc.Right)
            }.ToLookup(op => op.op);
            funcs = new List<string> { };
        }

        public RPNParser(string str)
        {
            StringExpression = str.Trim();
        }
        #endregion

        #region Properties
        public string StringExpression { get; private set; }
        #endregion

        #region Public
        public T Parse()
        {
            RPNBufs bufs = InitBufs();
            bufs.stringTokens = Tokenize();
            bufs.expression = RecognizeLexems(bufs.stringTokens);
            try
            {
                return CalculateExpression(bufs);
            }
            catch (InvalidOperationException)
            {
                throw new RPNParserException("Invalid expression format");
            }
        }
        #endregion

        #region Private

        #region Abstract methods
        protected abstract T Number(string str);
        protected abstract Queue<string> Tokenize();
        #endregion

        #region Algorithm
        protected Queue<RPNToken> RecognizeLexems(Queue<string> stringTokens)
        {
            var tokenQueue = new Queue<RPNToken>();
            TokenType tokenType;
            TokenType? prev = null;
            foreach (var token in stringTokens)
            {
                if (opInfoMap[token].Count() > 0)
                {
                    if (prev.Value == TokenType.CBracket ||
                        prev.Value == TokenType.Number)
                        tokenType = TokenType.BinOp;
                    else
                        tokenType = TokenType.UnOp;
                }
                else if (token == "(")
                    tokenType = TokenType.OBracket;
                else if (token == ")")
                    tokenType = TokenType.CBracket;
                else if (funcs.Contains(token))
                    tokenType = TokenType.Function;
                else
                    tokenType = TokenType.Number;
                prev = tokenType;
                tokenQueue.Enqueue(new RPNToken(token, tokenType));
            }
            return tokenQueue;
        }
        protected T CalculateExpression(RPNBufs bufs)
        {
            RPNToken currentToken;
            RPNToken tempToken;
            OpInfo curOpInfo;
            while (bufs.expression.Count > 0)
            {
                currentToken = bufs.expression.Dequeue();
                if (currentToken.tokenType == TokenType.Number)
                    bufs.result.Push(ToNumber(currentToken.str));
                else if (currentToken.tokenType == TokenType.Function)
                    bufs.buffer.Push(currentToken);
                else if (currentToken.tokenType == TokenType.BinOp ||
                    currentToken.tokenType == TokenType.UnOp)
                {
                    curOpInfo = GetOpInfo(currentToken);
                    while (bufs.buffer.Count() > 0)
                    {
                        RPNToken cmpToken = bufs.buffer.Peek();
                        if (cmpToken.tokenType == TokenType.Function ||
                             (IsOp(cmpToken) && CmpPriorities(GetOpInfo(cmpToken), curOpInfo) > 0))
                            CalcToken(bufs, bufs.buffer.Pop());
                        else
                            break;

                    }
                    bufs.buffer.Push(currentToken);
                }
                else if (currentToken.tokenType == TokenType.OBracket)
                    bufs.buffer.Push(currentToken);
                else if (currentToken.tokenType == TokenType.CBracket)
                {
                    while ((tempToken = bufs.buffer.Pop()).tokenType != TokenType.OBracket)
                        CalcToken(bufs, tempToken);
                    if (bufs.buffer.Count() > 0 && bufs.buffer.Peek().tokenType == TokenType.Function)
                        CalcToken(bufs, bufs.buffer.Pop());
                }
                else
                    throw new NotImplementedException("Unimplemented token");
            }
            while (bufs.buffer.Count() > 0)
                CalcToken(bufs, bufs.buffer.Pop());
            if (bufs.result.Count() > 1)
                throw new ArgumentException("Cannot calculate this expression. Remaining buf contains extra numbers.");
            else if (bufs.result.Count() == 0)
                return ToNumber("0");
            return bufs.result.Pop();
        }
        #endregion

        #region Private Algorithm tools
        private RPNBufs InitBufs()
        {
            var ret = new RPNBufs
            {
                expression = new Queue<RPNToken>(),
                buffer = new Stack<RPNToken>(),
                result = new Stack<T>()
            };
            return ret;
        }
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
            var curOps = opInfoMap[token.str];
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
        private int CmpPriorities(OpInfo left, OpInfo right)
        {
            if ((right.assoc == OpAssoc.Left && right.priority <= left.priority) ||
                (right.assoc == OpAssoc.Right && right.priority < left.priority))
                return 1;
            return -1;
        }
        #endregion        

        #region Protected Algorithm tools
        protected void CalcToken(RPNBufs bufs, RPNToken op)
        {
            if (op.tokenType == TokenType.Function)
                bufs.result.Push(CalcFunc(bufs.result.Pop(), op.str));
            else if (op.tokenType == TokenType.BinOp)
                bufs.result.Push(CalcBinaryOp(bufs.result.Pop(), bufs.result.Pop(), op.str));
            else if (op.tokenType == TokenType.UnOp)
                bufs.result.Push(CalcUnaryOp(bufs.result.Pop(), op.str));
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
        protected T CalcUnaryOp(T operand, string op)
        {
            switch (op)
            {
                case "+":
                    break;
                case "-":
                    operand.Negate();
                    break;
            }
            return operand;
        }
        protected virtual T CalcFunc(T arg, string func)
        {
            return arg;
        }
        #endregion

        #endregion
    }
}
