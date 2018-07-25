using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

//TODO: FABRIC => CONSTRUCTOR & EXCEPTION IF INVALID

namespace BignumArithmetics
{
    /// <summary>Сlass for fixed point 
    /// big numbers</summary>
    public class BigDecimal : BigNumber
    {
        #region Constructors
        /// <summary>Constructor creates a BigDecimal equal to 0</summary>
        /// <returns>An instance of BigDecimal</returns>
        public BigDecimal() { }
        /// <summary>Constructor creates a BigDecimal equal to parameter</summary>
        /// <param name="from">any BigDecimal</param>
        /// <returns>An instance of BigDecimal equal to parameter</returns>
        public BigDecimal(BigDecimal from)
        {
            CleanString = from.CleanString;
            if (from.Sign < 0)
                Negate();
            DotPos = from.DotPos;
            Fractional = from.Fractional;
        }
        /// <summary>Private constructor creates a BigDecimal from a valid string 
        /// that is matching <see cref="validStringRegEx"/> 
        /// and is cut with <see cref="cleanStringRegEx"/></summary>
        /// <param name="str">string representing number digits and delimiter</param>
        /// <param name="sign">integer representing number sign</param>
        /// <returns>An instance of BigDecimal</returns>
        /// <exception cref="ArgumentException">ArgumentException
        /// thrown in case of invalid input</exception>
        public BigDecimal(string str)
        {
            if (string.IsNullOrEmpty(str) ||
                string.IsNullOrEmpty(validStringRegEx.Match(str).Value))
                throw new ArgumentException("Invalid argument \"" + str + "\"");
            CleanAndSaveNumericString(str);
        }
        #endregion

        #region Static Methods
        /// <summary>Converts numbers into matching char symbols</summary>
        /// <param name="digit">Number to be converted. Must be in [0..9] range</param>
        /// <returns>A matching char; '0' if digit does not match limits</returns>
        public static char ToChar(int digit)
        {
            if (digit >= 0 && digit < 10)
                return digit.ToString()[0];
            return '0';
        }
        /// <summary>Converts characters to matching number</summary>
        /// <param name="c">Character to be converted. Must consist of ['0'..'9']</param>
        /// <returns>A matching number; -1 if parameter does not match limits.</returns>
        public static int ToDigit(char c)
        {
            if (Char.IsDigit(c))
                return Convert.ToInt32(c - '0');
            return -1;
        }
        /// <summary>Fabric thar returns an instance of BigDecimal constructed from a number</summary>
        /// <param name="number">Object of any numeric type</param>
        /// <returns>An instance of BigDecimal. null if parameter is invalid</returns>
        public static BigDecimal CreateFromNumber<T>(T number)
        {
            string str = number.ToString();
            string sysDelim = System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            str = str.Replace(sysDelim, delimiter);
            return new BigDecimal(str);
        }
        /// <summary>Сonverts BigDecimal into reversed digit list with padding zeroes</summary>
        /// <param name="desiredInt">int represening how many digits should integer part contain</param>
        /// <param name="desiredFrac">int represening how many digits should fractional part contain</param>
        /// <returns>List of digits</returns>
        public static List<int> BigDecimalToIntList(BigDecimal num, int desiredInt = 0, int desiredFrac = 0)
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
        /// <summary>CleanNumericString method cleans digit string with <see cref="cleanStringRegEx"/></summary>
        /// <param name="rawString">String representing of digits</param>
        /// <param name="sign">Integer representing position of dot in cleaned string</param>
        /// <returns>A clean string; "0" in case of invalid arguments</returns>
        private void CleanAndSaveNumericString(string rawString)
        {
            string substr;

            substr = cleanStringRegEx.Match(rawString).Value;
            if (substr == "")
            {
                CleanString = "0";
                return;
            }
            CleanString = substr;
            if (rawString.Contains("-"))
                Negate();
        }
        #endregion

        #region Public tools
        /// <summary>Returns a copy of BigDecimal instance </summary>
        /// <returns>Copy of BigDecimal instance</returns>
        public BigDecimal Copy()
        {
            return new BigDecimal(this);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            return (this == obj as BigDecimal);
        }
        /// <summary> Returns |BigDecimal| </summary>
        /// <param name="bf">BigDecimal parameter</param>
        /// <returns>|BigDecimal|</returns>
        public static BigDecimal Abs(BigDecimal bf)
        {
            BigDecimal ret = new BigDecimal(bf);
            if (bf.Sign < 0)
                bf.Negate();
            return bf;
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

        /// <summary>Method that is calculating (this + op) for BigDecimal objects.
        /// Overrided from parent</summary>
        /// <param name="op">Second operand</param>
        /// <returns>BigDecimal equal to (this + op)  upcasted to BigNumber</returns>
        public override BigNumber Add(BigNumber op)
        {
            if (!(op is BigDecimal))
                throw new ArgumentException("Cannot Add BigDecimal to " + op.GetType());

            BigDecimal bfLeft = this;
            BigDecimal bfRight = (BigDecimal)op;

            if (bfLeft.Sign != bfRight.Sign)
                return bfLeft.Substract(-bfRight);

            int desiredInt = Math.Max(bfLeft.Integer, bfRight.Integer);
            int desiredFrac = Math.Max(bfLeft.Fractional, bfRight.Fractional);
            var leftList = BigDecimalToIntList(bfLeft, desiredInt, desiredFrac);
            var rightList = BigDecimalToIntList(bfRight, desiredInt, desiredFrac);
            var resultList = leftList.SumWithList(rightList);
            NormalizeList(resultList);

            BigDecimal bfAns = new BigDecimal(IntListToString(resultList, resultList.Count - desiredFrac));
            if (Sign < 0)
                bfAns.Negate();
            return bfAns;
        }
        /// <summary>Method that is calculating (this - op) for BigDecimal objects.
        /// Overrided from parent</summary>
        /// <param name="op">Second operand</param>
        /// <returns>BigDecimal equal to (this - op)  upcasted to BigNumber</returns>
        public override BigNumber Substract(BigNumber op)
        {
            if (!(op is BigDecimal))
                throw new ArgumentException("Cannot Add BigDecimal and " + op.GetType());

            BigDecimal bfLeft = this;
            BigDecimal bfRight = (BigDecimal)op;

            if (bfLeft.Sign > 0 && bfRight.Sign < 0)
                return bfLeft.Add(-bfRight);
            if (bfLeft.Sign < 0 && bfRight.Sign > 0)
                return -(BigDecimal)bfRight.Add(-bfLeft);
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
            var leftList = BigDecimalToIntList(bfLeft, desiredInt, desiredFrac);
            var rightList = BigDecimalToIntList(bfRight, desiredInt, desiredFrac);
            var resultList = leftList.SubByList(rightList);
            NormalizeList(resultList);

            BigDecimal bfAns = new BigDecimal(IntListToString(resultList, resultList.Count - desiredFrac));
            if (sign < 0)
                bfAns.Negate();
            return bfAns;
        }
        /// <summary>Method that is calculating (this * op) for BigDecimal objects.
        /// Overrided from parent</summary>
        /// <param name="op">Second operand</param>
        /// <returns>BigDecimal equal to (this * op)  upcasted to BigNumber</returns>
        public override BigNumber Multiply(BigNumber op)
        {
            if (!(op is BigDecimal))
                throw new ArgumentException("Cannot Add BigDecimal and " + op.GetType());

            BigDecimal bfLeft = this;
            BigDecimal bfRight = (BigDecimal)op;
            
            if (bfLeft.Integer + bfLeft.Fractional < bfRight.Integer + bfRight.Fractional)
                Swap(ref bfLeft, ref bfRight);
            int newDot = bfLeft.Fractional + bfRight.Fractional;
            var leftList = BigDecimalToIntList(bfLeft);
            var rightList = BigDecimalToIntList(bfRight);
            var resultList = leftList.MulWithList(rightList);
            NormalizeList(resultList);

            BigDecimal bfAns = new BigDecimal(IntListToString(resultList, resultList.Count - newDot));
            if (bfLeft.Sign * bfRight.Sign < 0)
                bfAns.Negate();
            return bfAns;
        }
        /// <summary>Method that is calculating (this / op) for BigDecimal objects.
        /// Overrided from parent</summary>
        /// <param name="op">Second operand</param>
        /// <returns>BigDecimal equal to (this / op)  upcasted to BigNumber</returns>
        public override BigNumber Divide(BigNumber op)
        {
            if (!(op is BigDecimal))
                throw new ArgumentException("Cannot Add BigDecimal and " + op.GetType());

            if (op.CleanString == "0")
                throw new DivideByZeroException();
            BigDecimal bfLeft = this;
            BigDecimal bfRight = (BigDecimal)op;

            int multiplier = Math.Max(bfLeft.Fractional, bfRight.Fractional);
            var leftList = BigDecimalToIntList(bfLeft, 0, multiplier + FracPrecision);
            var rightList = BigDecimalToIntList(bfRight, 0, multiplier);
            leftList.RemoveTailingZeros();
            rightList.RemoveTailingZeros();

            List<int> resultList = leftList.DivByList(rightList, NormalizeList, out List<int> subList);
            int dotPos = resultList.Count - FracPrecision;

            BigDecimal bfAns = new BigDecimal(IntListToString(resultList, dotPos));
            if (bfLeft.Sign * bfRight.Sign < 0)
                bfAns.Negate();
            return bfAns;
        }
        /// <summary>Method that is calculating (this % op) for BigDecimal objects.
        /// Overrided from parent</summary>
        /// <param name="op">Second operand</param>
        /// <returns>BigDecimal equal to (this % op)  upcasted to BigNumber</returns>
        public override BigNumber Mod(BigNumber op)
        {
            if (!(op is BigDecimal))
                throw new ArgumentException("Cannot Add BigDecimal and " + op.GetType());

            if (op.CleanString == "0")
                throw new ArgumentException("Cannot calculate BigDecimal % 0");
            BigDecimal bfLeft = this;
            BigDecimal bfRight = (BigDecimal)op;

            int temp = FracPrecision;
            FracPrecision = 0;
            BigDecimal bfDiv = bfLeft / bfRight;
            FracPrecision = temp;
            BigDecimal bfAns = bfLeft - bfDiv * bfRight;
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
        public static BigDecimal operator -(BigDecimal num)
        {
            BigDecimal ret = new BigDecimal(num);

            ret.Negate();
            return ret;
        }

        public static BigDecimal operator +(BigDecimal left, BigDecimal right)
        {
            if (left is null || right is null)
                return null;
            return (BigDecimal)left.Add(right);
        }
        public static BigDecimal operator -(BigDecimal left, BigDecimal right)
        {
            if (left is null || right is null)
                return null;
            return (BigDecimal)left.Substract(right);
        }
        public static BigDecimal operator *(BigDecimal left, BigDecimal right)
        {
            if (left is null || right is null)
                return null;
            return (BigDecimal)left.Multiply(right);
        }
        public static BigDecimal operator /(BigDecimal left, BigDecimal right)
        {
            if (left is null || right is null)
                return null;
            return (BigDecimal)left.Divide(right);
        }
        public static BigDecimal operator %(BigDecimal left, BigDecimal right)
        {
            if (left is null || right is null)
                return null;
            return (BigDecimal)left.Mod(right);
        }

        public static bool operator >(BigDecimal left, BigDecimal right)
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
        public static bool operator <(BigDecimal left, BigDecimal right)
        {
            return (!(left > right));
        }
        public static bool operator ==(BigDecimal left, BigDecimal right)
        {
            if (string.Compare(left.CleanString, right.CleanString) != 0 
                || left.Sign != right.Sign)
                return false;
            return true;
        }
        public static bool operator !=(BigDecimal left, BigDecimal right)
        {
            if (string.Compare(left.ToString(), right.ToString()) != 0)
                return false;
            return true;
        }

        public static explicit operator BigInteger(BigDecimal bf)
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
        private static readonly Regex validStringRegEx = new
            Regex(@"^\s*[+-]?\d+(\.\d+)?\s*$", RegexOptions.Compiled);
        /// <summary>cleanStringRegEx is a string representing RegEx
        /// used to clean valid input string in fabric method <see cref="CreateFromString"/></summary>
        private static readonly Regex cleanStringRegEx = 
            new Regex(@"([1-9]+[0-9]*(\.[0-9]*[1-9]+)?|0\.[0-9]*[1-9]+)", RegexOptions.Compiled);
        /// <summary>The _fracPrecision local field represents 
        /// a number of fractional digits counted while division for BigDecimal instances.</summary>
        private static volatile int _fracPrecision = 20;

        /// <summary>The _dotPos local field represents a delimiter position 
        /// of BigDecimal instance in string format.</summary>
        private volatile int _dotPos = 0;
        /// <summary>The _fracLen local field represents a number of digits 
        /// in the fractional part of BigDecimal instance.</summary>
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
        /// <summary>The DotPos property represents a delimiter position of BigDecimal instance in string format.</summary>
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
        /// in the integer part of BigDecimal instance.</summary>
        /// <value>The Integer property gets the value of the int property, <see cref="DotPos"/></value>
        public int Integer
        {
            get
            {
                return DotPos;
            }
        }
        /// <summary>The Fractional property represents a number of digits 
        /// in the fractional part of BigDecimal instance.</summary>
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
        /// a number of fractional digits counted while division for BigDecimal instances.</summary>
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
