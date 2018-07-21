using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

namespace BignumArithmetics
{
    /// <summary>Сlass for fixed point 
    /// big numbers</summary>
    public class BigFloat : BigNumber
    {
        #region Constructors
        /// <summary>Constructor creates a BigFLoat equal to 0</summary>
        /// <returns>An instance of BigFloat</returns>
        public BigFloat()
        {
            CleanString = "0";
        }
        /// <summary>Constructor creates a BigFloat equal to parameter</summary>
        /// <param name="from">any BigFloat</param>
        /// <returns>An instance of BigFloat equal to parameter</returns>
        public BigFloat(BigFloat from)
        {
            CleanString = from.CleanString;
            if (from.Sign < 0)
                Negate();
            DotPos = from.DotPos;
            Fractional = from.Fractional;
        }
        /// <summary>Private constructor creates a BigFloat from a valid string 
        /// that is matching <see cref="validStringRegEx"/> 
        /// and is cut with <see cref="cleanStringRegEx"/></summary>
        /// <param name="str">string representing number digits and delimiter</param>
        /// <param name="sign">integer representing number sign</param>
        /// <returns>An instance of BigFloat</returns>
        private BigFloat(string str, int sign)
        {
            CleanString = str;
            if (sign < 0)
                Negate();
        }
        #endregion

        #region Static Methods
        /// <summary>Fabric thar returns an instance of BigFloat constructed from a string
        /// that is matching <see cref="validStringRegEx"/> 
        /// and is cut with <see cref="cleanStringRegEx"/></summary>
        /// <param name="str">String that represents a number</param>
        /// <returns>An instance of BigFloat. null if parameter is invalid</returns>
        public static BigFloat CreateFromString(string str)
        {
            if (string.IsNullOrEmpty(str) ||
                !Regex.IsMatch(str, validStringRegEx, RegexOptions.None))
                return new BigFloat();
            str = CleanNumericString(str, out int sign);
            return new BigFloat(str, sign);
        }
        /// <summary>Fabric thar returns an instance of BigFloat constructed from a number</summary>
        /// <param name="number">Object of any numeric type</param>
        /// <returns>An instance of BigFloat. null if parameter is invalid</returns>
        public static BigFloat CreateFromNumber<T>(T number)
        {
            string str = number.ToString();
            string sysDelim = System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            str = str.Replace(sysDelim, delimiter);
            return CreateFromString(str);
        }
        /// <summary>Сonverts BigFloat into reversed digit list with padding zeroes</summary>
        /// <param name="desiredInt">int represening how many digits should integer part contain</param>
        /// <param name="desiredFrac">int represening how many digits should fractional part contain</param>
        /// <returns>List of digits</returns>
        public static List<int> BigNumberToIntList(BigFloat num, int desiredInt = 0, int desiredFrac = 0)
        {
            if (num is null)
                return null;

            List<int> ret = new List<int>();
            int IntZeros, FracZeros;

            IntZeros = Math.Max(num.Integer, desiredInt) - num.Integer;
            FracZeros = Math.Max(num.Fractional, desiredFrac) - num.Fractional;
            ret.AddRange(Enumerable.Repeat(0, FracZeros));
            for (int i = num.CleanString.Length - 1; i >= 0; i--)
                if (num.CleanString[i] != '.')
                    ret.Add(ToDigit(num.CleanString[i]));
            ret.AddRange(Enumerable.Repeat(0, IntZeros));
            return ret;
        }
        /// <summary>IntListToString method converts digit list to string</summary>
        /// <param name="digits">List of digits</param>
        /// <param name="dotPos">Integer representing reversed position of dot in string</param>
        /// <returns>A string representing a number; null in case of invalid arguments</returns>
        public static string IntListToString(List<int> digits, int dotPos = 0)
        {
            int i;
            int reverseDot;
            StringBuilder sb;

            if (digits is null || digits.Count == 0)
                return "";
            if (dotPos < 0)
                dotPos = 0;
            sb = new StringBuilder();
            reverseDot = digits.Count - dotPos;
            for (i = digits.Count - 1; i >= reverseDot; i--)
                sb.Append(ToChar(digits[i]));
            if (i >= 0)
            {
                sb.Append(delimiter);
                while (i >= 0)
                    sb.Append(ToChar(digits[i--]));
            }
            return sb.ToString();
        }
        /// <summary> Returns |BigFloat| </summary>
        /// <param name="bf">BigFloat parameter</param>
        /// <returns>|BigFloat|</returns>
        public static BigFloat Abs(BigFloat bf)
        {
            BigFloat ret = new BigFloat(bf);
            if (bf.Sign < 0)
                bf.Negate();
            return bf;
        }
        /// <summary>CleanNumericString method cleans digit string with <see cref="cleanStringRegEx"/></summary>
        /// <param name="RawString">String representing of digits</param>
        /// <param name="sign">Integer representing position of dot in cleaned string</param>
        /// <returns>A clean string; "0" in case of invalid arguments</returns>
        private static string CleanNumericString(string RawString, out int sign)
        {
            string substr;

            if (RawString is null)
            {
                sign = 1;
                return "0";
            }
            substr = Regex.Match(RawString, cleanStringRegEx).Value;
            if (substr == "")
            {
                sign = 1;
                return "0";
            }
            sign = RawString.Contains("-") ? -1 : 1;
            return substr;
        }
        #endregion

        #region Public tools
        /// <summary>Returns a copy of BigFloat instance </summary>
        /// <returns>Copy of BigFloat instance</returns>
        public BigFloat Copy()
        {
            return new BigFloat(this);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            return (this == obj as BigFloat);
        }
        #endregion
     
        #region Private Methods
        /// <summary>Calculates a position of delimiter
        /// in <see cref="CleanString"/></summary>
        private void FindDotPos()
        {
            if (_dotPos > 0)
                return;
            _dotPos = CleanString.IndexOf(delimiter);
            if (_dotPos < 0)
                _dotPos = CleanString.Length;
        }
        #endregion

        #region Parent Overrides
        /// <summary>Normalizes reversed list of digits</summary>
        /// <param name="digits">List of digits</param>
        public override void NormalizeList(List<int> digits)
        {
            int i;

            if (digits is null || digits.Count == 0)
                return;
            for (i = 0; i < digits.Count - 1; i++)
            {
                if (digits[i] < 0)
                {
                    //todo: if i + 1 >= Count then error
                    digits[i] += 10;
                    digits[i + 1]--;
                }
                else
                {
                    digits[i + 1] += digits[i] / 10;
                    digits[i] %= 10;
                }
            }
            while (digits[i] > 9)
            {
                digits.Add(digits[i] / 10);
                digits[i] %= 10;
                i++;
            }
        }

        /// <summary>Method that is calculating (this + op) for BigFLoat objects.
        /// Overrided from parent</summary>
        /// <param name="op">Second operand</param>
        /// <returns>BigFloat equal to (this + op)  upcasted to BigNumber</returns>
        public override BigNumber Add(BigNumber op)
        {
            if (!(op is BigFloat))
                throw new ArgumentException("Cannot Add BigFloat to " + op.GetType());

            BigFloat bfLeft = this;
            BigFloat bfRight = (BigFloat)op;

            if (bfLeft.Sign != bfRight.Sign)
                return bfLeft.Substract(-bfRight);

            int desiredInt = Math.Max(bfLeft.Integer, bfRight.Integer);
            int desiredFrac = Math.Max(bfLeft.Fractional, bfRight.Fractional);
            var leftList = BigNumberToIntList(bfLeft, desiredInt, desiredFrac);
            var rightList = BigNumberToIntList(bfRight, desiredInt, desiredFrac);
            var resultList = SumTwoLists(leftList, rightList);

            BigFloat bfAns = CreateFromString(IntListToString(resultList, resultList.Count - desiredFrac));
            if (Sign < 0)
                bfAns.Negate();
            return bfAns;
        }
        /// <summary>Method that is calculating (this - op) for BigFLoat objects.
        /// Overrided from parent</summary>
        /// <param name="op">Second operand</param>
        /// <returns>BigFloat equal to (this - op)  upcasted to BigNumber</returns>
        public override BigNumber Substract(BigNumber op)
        {
            if (!(op is BigFloat))
                throw new ArgumentException("Cannot Add BigFloat and " + op.GetType());

            BigFloat bfLeft = this;
            BigFloat bfRight = (BigFloat)op;

            if (bfLeft.Sign > 0 && bfRight.Sign < 0)
                return bfLeft.Add(-bfRight);
            if (bfLeft.Sign < 0 && bfRight.Sign > 0)
                return -(BigFloat)bfRight.Add(-bfLeft);
            if (bfLeft.Sign < 0 && bfRight.Sign < 0)
                return (-bfRight).Substract(-bfLeft);
            //both operands are > 0 here
            int sign = 1;

            if (bfLeft < bfRight)
            {
                sign = -sign;
                Swap(ref bfLeft, ref bfRight);
            }
            int desiredInt = Math.Max(bfLeft.Integer, bfRight.Integer);
            int desiredFrac = Math.Max(bfLeft.Fractional, bfRight.Fractional);
            var leftList = BigNumberToIntList(bfLeft, desiredInt, desiredFrac);
            var rightList = BigNumberToIntList(bfRight, desiredInt, desiredFrac);
            var resultList = DifTwoLists(leftList, rightList);
        
            BigFloat bfAns = CreateFromString(IntListToString(resultList, resultList.Count - desiredFrac));
            if (sign < 0)
                bfAns.Negate();
            return bfAns;
        }
        /// <summary>Method that is calculating (this * op) for BigFLoat objects.
        /// Overrided from parent</summary>
        /// <param name="op">Second operand</param>
        /// <returns>BigFloat equal to (this * op)  upcasted to BigNumber</returns>
        public override BigNumber Multiply(BigNumber op)
        {
            if (!(op is BigFloat))
                throw new ArgumentException("Cannot Add BigFloat and " + op.GetType());

            BigFloat bfLeft = this;
            BigFloat bfRight = (BigFloat)op;
            
            if (bfLeft.Integer + bfLeft.Fractional < bfRight.Integer + bfRight.Fractional)
                Swap(ref bfLeft, ref bfRight);
            int newDot = bfLeft.Fractional + bfRight.Fractional;
            var leftList = BigNumberToIntList(bfLeft);
            var rightList = BigNumberToIntList(bfRight);
            var resultList = MulTwoLists(leftList, rightList, true);

            BigFloat bfAns = CreateFromString(IntListToString(resultList, resultList.Count - newDot));
            if (bfLeft.Sign * bfRight.Sign < 0)
                bfAns.Negate();
            return bfAns;
        }
        /// <summary>Method that is calculating (this / op) for BigFLoat objects.
        /// Overrided from parent</summary>
        /// <param name="op">Second operand</param>
        /// <returns>BigFloat equal to (this / op)  upcasted to BigNumber</returns>
        public override BigNumber Divide(BigNumber op)
        {
            if (!(op is BigFloat))
                throw new ArgumentException("Cannot Add BigFloat and " + op.GetType());

            if (op.CleanString == "0")
                throw new DivideByZeroException();
            BigFloat bfLeft = this;
            BigFloat bfRight = (BigFloat)op;

            int multiplier = Math.Max(bfLeft.Fractional, bfRight.Fractional);
            var leftList = BigNumberToIntList(bfLeft, 0, multiplier + FracPrecision);
            var rightList = BigNumberToIntList(bfRight, 0, multiplier);
            RemoveTailingZeros(leftList);
            RemoveTailingZeros(rightList);

            List<int> resultList = DivTwoLists(leftList, rightList, out List<int> subList);
            int dotPos = resultList.Count - FracPrecision;

            BigFloat bfAns = CreateFromString(IntListToString(resultList, dotPos));
            if (bfLeft.Sign * bfRight.Sign < 0)
                bfAns.Negate();
            return bfAns;
        }
        /// <summary>Method that is calculating (this % op) for BigFLoat objects.
        /// Overrided from parent</summary>
        /// <param name="op">Second operand</param>
        /// <returns>BigFloat equal to (this % op)  upcasted to BigNumber</returns>
        public override BigNumber Mod(BigNumber op)
        {
            if (!(op is BigFloat))
                throw new ArgumentException("Cannot Add BigFloat and " + op.GetType());

            if (op.CleanString == "0")
                throw new ArgumentException("Cannot calculate BigFloat % 0");
            BigFloat bfLeft = this;
            BigFloat bfRight = (BigFloat)op;

            int temp = FracPrecision;
            FracPrecision = 0;
            BigFloat bfDiv = bfLeft / bfRight;
            FracPrecision = temp;
            BigFloat bfAns = bfLeft - bfDiv * bfRight;
            return bfAns;
        }

        /// <summary>Indexer allowing to get indexed digit values</summary>
        /// <param name="index">Integer representing index</param>
        /// <returns>Integer representing digit</returns>
        public override int this[int index]
        {
            get
            {
                if (index < 0 || index >= (CleanString.Length - (DotPos == CleanString.Length ? 0 : 1)))
                    return -1;
                return ToDigit(CleanString[index - (index >= DotPos ? 1 : 0)]);
            }
        }
        #endregion

        #region Operators
        public static BigFloat operator -(BigFloat num)
        {
            BigFloat ret = new BigFloat(num);

            ret.Negate();
            return ret;
        }

        public static BigFloat operator +(BigFloat left, BigFloat right)
        {
            if (left is null || right is null)
                return null;
            return (BigFloat)left.Add(right);
        }
        public static BigFloat operator -(BigFloat left, BigFloat right)
        {
            if (left is null || right is null)
                return null;
            return (BigFloat)left.Substract(right);
        }
        public static BigFloat operator *(BigFloat left, BigFloat right)
        {
            if (left is null || right is null)
                return null;
            return (BigFloat)left.Multiply(right);
        }
        public static BigFloat operator /(BigFloat left, BigFloat right)
        {
            if (left is null || right is null)
                return null;
            return (BigFloat)left.Divide(right);
        }
        public static BigFloat operator %(BigFloat left, BigFloat right)
        {
            if (left is null || right is null)
                return null;
            return (BigFloat)left.Mod(right);
        }

        public static bool operator >(BigFloat left, BigFloat right)
        {
            if (left.Sign > 0)
            {
                if (right.Sign < 0)
                    return true;
                if (left.Integer > right.Integer)
                    return true;
                if (left.Integer < right.Integer)
                    return false;
                if (string.Compare(left.CleanString, right.CleanString) > 0)
                    return true;
                return false;
            }
            if (right.Sign > 0)
                return false;
            if (left.Integer > right.Integer)
                return false;
            if (left.Integer < right.Integer)
                return true;
            if (string.Compare(left.CleanString, right.CleanString) > 0)
                return false;
            return true;
        }
        public static bool operator <(BigFloat left, BigFloat right)
        {
            return (!(left > right));
        }
        public static bool operator ==(BigFloat left, BigFloat right)
        {
            if (string.Compare(left.CleanString, right.CleanString) != 0 
                || left.Sign != right.Sign)
                return false;
            return true;
        }
        public static bool operator !=(BigFloat left, BigFloat right)
        {
            if (string.Compare(left.ToString(), right.ToString()) != 0)
                return false;
            return true;
        }

        public static explicit operator BigInteger(BigFloat bf)
        {
            string intString = bf.CleanString.Substring(0, bf.DotPos);
            return BigInteger.CreateFromString(intString);
        }
        #endregion

        #region Variables
        /// <summary>delimiter represents a symbol or string that splits number
        /// into integer and fractional parts </summary>
        private static readonly string delimiter = ".";
        /// <summary>validStringRegEx is a string representing RegEx
        /// used to validate input string in fabric method <see cref="CreateFromString"/>
        /// into integer and fractional parts </summary>
        private static readonly string validStringRegEx = @"^\s*[+-]?[0-9]+(\.[0-9]+)?\s*$";
        /// <summary>cleanStringRegEx is a string representing RegEx
        /// used to clean valid input string in fabric method <see cref="CreateFromString"/></summary>
        private static readonly string cleanStringRegEx = @"([1-9]+[0-9]*(\.[0-9]*[1-9]+)?|0\.[0-9]*[1-9]+)";
        /// <summary>The _fracPrecision local field represents 
        /// a number of fractional digits counted while division for BigFloat instances.</summary>
        private static volatile int _fracPrecision = 20;

        /// <summary>The _dotPos local field represents a delimiter position 
        /// of BigFloat instance in string format.</summary>
        private volatile int _dotPos = 0;
        /// <summary>The _fracLen local field represents a number of digits 
        /// in the fractional part of BigFloat instance.</summary>
        private volatile int _fracLen = -1;
        #endregion

        #region Mutexes
        ///<summary>Random variable used for thread-safe <see cref="DotPos"/>
        ///property first calculation </summary>
        private readonly Object dotPosMutex = new Object();
        ///<summary>Random variable used for thread-safe <see cref="Fractional"/>
        ///property first calculation </summary>
        private readonly Object fracLenMutex = new Object();
        #endregion

        #region Properties
        /// <summary>The DotPos property represents a delimiter position of BigFloat instance in string format.</summary>
        /// <value>The DotPos property gets/private sets the value of the int field, <see cref="_dotPos"/></value>
        public int DotPos
        {
            get
            {
                if (_dotPos == 0)
                {
                    lock (dotPosMutex)
                    {
                        FindDotPos();
                    }
                }
                return _dotPos;
            }
            private set
            {
                _dotPos = value;
            }
        }
        /// <summary>The Integer property represents a number of digits 
        /// in the integer part of BigFloat instance.</summary>
        /// <value>The Integer property gets the value of the int property, <see cref="DotPos"/></value>
        public int Integer
        {
            get
            {
                return DotPos;
            }
        }
        /// <summary>The Fractional property represents a number of digits 
        /// in the fractional part of BigFloat instance.</summary>
        /// <value>The Fractional property gets/private sets the value of the int property, <see cref="_fracLen"/></value>
        public int Fractional
        {
            get
            {
                if (_fracLen < 0)
                    lock (fracLenMutex)
                    {
                        _fracLen = CleanString.Length - DotPos;
                        if (_fracLen > 0)
                            _fracLen--;
                    }
                return _fracLen;
            }
            private set
            {
                _fracLen = value;
            }
        }
        /// <summary>The FracPrecision property represents 
        /// a number of fractional digits counted while division for BigFloat instances.</summary>
        /// <value>The FracPrecision property gets/sets the value of the int property, <see cref="_fracPrecision"/></value>
        public static int FracPrecision
        {
            get
            {
                return _fracPrecision;
            }
            set
            {
                if (value < 0)
                    _fracPrecision = 0;
                else
                    _fracPrecision = value;
            }
        }
        #endregion
    }
}
