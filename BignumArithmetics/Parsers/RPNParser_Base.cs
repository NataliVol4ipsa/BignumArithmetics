using System;
using System.Linq;
using System.Collections.Generic;

namespace BignumArithmetics.Parsers
{
    /// <summary> Abstract class that parses BigNumber child string expression 
    /// using Shunting-yard algorithm and calculates it </summary>
    /// <typeparam name="T">Any class inheriting BigNumber</typeparam>
    public abstract class RPNParser<T> where T : BigNumber
    {
        #region Enums
        /// <summary>Enum for token types</summary>
        protected enum TokenType
        {
            BinOp,
            UnOp,
            OBracket,
            CBracket,
            Number,
            Function
        }
        /// <summary>Enum for token arity</summary>
        protected enum OpArity { Unary, Binary }
        /// <summary>Enum for token association type</summary>
        protected enum OpAssoc { Left, Right }
        #endregion

        #region Structs
        /// <summary>Structure describing token</summary>
        protected struct RPNToken
        {
            public RPNToken(string str, TokenType tokenType)
            {
                this.str = str;
                this.tokenType = tokenType;
            }
            /// <summary>Type of token</summary>
            public TokenType tokenType;
            /// <summary>String representing token</summary>
            public string str;
        }
        /// <summary>Structure describing operation</summary>
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
        /// <summary>Structure storing RPN convertion and calculation buffers</summary>
        protected struct RPNBufs
        {
            /// <summary>Queue of input string's lexems </summary>
            public Queue<string> stringTokens;
            /// <summary>Queue of <see cref="stringTokens"/> recognised as RPNTokens </summary>
            public Queue<RPNToken> expression;
            /// <summary>Stack used as buffer for shunting-yard algorithm</summary>
            public Stack<RPNToken> buffer;
            /// <summary>Stack of result values</summary>
            public Stack<T> result;
        }
        #endregion

        #region Variables

        #region Static Const
        /// <summary>String representing format of token generic template 
        /// used by child classes</summary>
        protected static readonly string regexFormat;
        #endregion

        #region Static
        /// <summary>ILookup providing quick search through operations' OpInfo</summary>
        protected static ILookup<string, OpInfo> opInfoMap;
        /// <summary>List representing available functions. Filled by children</summary>
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
        #endregion

        #region Public
        /// <summary> Method is parsing string expression 
        /// and returning result of its calculation</summary>
        /// <param name="str">string representing <see cref="T"/> expression</param>
        /// <returns>Instance representing calculation result</returns>
        /// <exception cref="RPNParserException">Thrown in case of invalid expression</exception>
        public T Parse(string str)
        {
            RPNBufs bufs = InitBufs();
            bufs.stringTokens = Tokenize(str);
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
        /// <summary>Abstract method that creates
        /// a new instance of <see cref="T"/> created from string </summary>
        /// <param name="str">String representing <see cref="T"/> number</param>
        /// <returns> An instance of <see cref="T"/></returns>
        protected abstract T Number(string str);
        /// <summary> Abstract method that splits a string into lexems </summary>
        /// <param name="str">String to be tokenized</param>
        /// <returns>Queue of lexems</returns>
        protected abstract Queue<string> Tokenize(string str);
        #endregion

        #region Algorithm
        /// <summary>RecognizeLexems recognizes array of lexems as different token types</summary>
        /// <param name="stringTokens">Array of lexems to be analyzed</param>
        /// <returns>Queue of recognized tokens</returns>
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
        /// <summary> Calculates the string expression </summary>
        /// <param name="bufs">Buffers required by algorythm</param>
        /// <returns></returns>
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
        /// <summary>Does RPNBuf instanciation</summary>
        /// <returns>A new instance of RPNBuf</returns>
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
        /// <summary>Converting string to <see cref="T"/> with exception</summary>
        /// <param name="str">string representing number</param>
        /// <returns>Instance of <see cref="T"/></returns>
        /// <exception cref="RPNParserException">Thrown in case of
        /// <see cref="T"/> constructor throwing exception</exception>
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
        /// <summary>Does search of OpInfo in <see cref="opInfoMap"/></summary>
        /// <param name="token">Token to be searched info about</param>
        /// <returns>OpInfo about token</returns>
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
        /// <summary> Checks if token is unary or binaty operation</summary>
        /// <param name="t">Token to be checked</param>
        /// <returns>Bool representing if token is operation</returns>
        private bool IsOp(RPNToken t)
        {
            if (t.tokenType == TokenType.BinOp || t.tokenType == TokenType.UnOp)
                return true;
            return false;
        }
        /// <summary> Compares priorities of two operators</summary>
        /// <param name="left">Left operator</param>
        /// <param name="right">Right operator</param>
        /// <returns>Integer representing order of left operator 
        ///  compared to right operator in priority list</returns>
        private int CmpPriorities(OpInfo left, OpInfo right)
        {
            if ((right.assoc == OpAssoc.Left && right.priority <= left.priority) ||
                (right.assoc == OpAssoc.Right && right.priority < left.priority))
                return 1;
            return -1;
        }
        #endregion        

        #region Protected Algorithm tools
        /// <summary> Calculates single token operation or function </summary>
        /// <param name="bufs">Buffers used by algorithm</param>
        /// <param name="op">Token representing action</param>
        protected void CalcToken(RPNBufs bufs, RPNToken op)
        {
            if (op.tokenType == TokenType.Function)
                bufs.result.Push(CalcFunc(bufs.result.Pop(), op.str));
            else if (op.tokenType == TokenType.BinOp)
                bufs.result.Push(CalcBinaryOp(bufs.result.Pop(), bufs.result.Pop(), op.str));
            else if (op.tokenType == TokenType.UnOp)
                bufs.result.Push(CalcUnaryOp(bufs.result.Pop(), op.str));
        }
        /// <summary> Calculates binary operation </summary>
        /// <param name="right">Right operand</param>
        /// <param name="left">Left operand</param>
        /// <param name="op">String representing operator</param>
        /// <returns>Result of operation applied to left and right</returns>
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
        /// <summary> Calculates unary operation </summary>
        /// <param name="operand">Operand</param>
        /// <param name="op">String representing operator</param>
        /// <returns>Result of operation applied to operand</returns>
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
        /// <summary> Calls function with parameter</summary>
        /// <param name="arg">Parameter of function</param>
        /// <param name="func">String representing function</param>
        /// <returns>Result of function call</returns>
        protected virtual T CalcFunc(T arg, string func)
        {
            return arg;
        }
        #endregion

        #endregion
    }
}
