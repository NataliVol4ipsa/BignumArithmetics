using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

//TODO: TEST BIGBASED
//todo: add array of regexes for each base
//TODO: FABRIC => CONSTRUCTOR & EXCEPTION IF INVALID

namespace BignumArithmetics
{
    /// <summary>Сlass for 2-16 based 
    /// big numbers</summary>
    public class BigBased : BigNumber
    {
        #region Constructors
        /// <summary>Constructor creates a BigBased equal to 0 in base 10</summary>
        /// <returns>An instance of BigBased</returns>
        public BigBased(int bigBase = 10)
        {
            if (bigBase < 2 || bigBase > 16)
                Base = 10;
            else
                Base = bigBase;
        }
        /// <summary>Constructor creates a BigBased equal to parameter</summary>
        /// <param name="from">any BigBased</param>
        /// <returns>An instance of BigBased equal to parameter</returns>
        public BigBased(BigBased from)
        {
            CleanString = from.CleanString;
            if (from.Sign < 0)
                Negate();
            Base = from.Base;
        }
        /// <summary>Private constructor creates a BigBased from a valid string 
        /// that is matching <see cref="validStringRegEx"/> 
        /// and is cut with <see cref="cleanStringRegEx"/>
        /// of base bigBase </summary>
        /// <param name="str">string representing number digits and delimiter</param>
        /// <param name="sign">integer representing number sign</param>
        /// <param name="bigBase">2..16 base of number</param>
        /// <returns>An instance of BigBased</returns>
        private BigBased(string str, int sign, int bigBase)
        {
            CleanString = str;
            if (sign < 0)
                Negate();
            Base = bigBase;
        }
        #endregion

        #region Static Methods
        /// <summary>Fabric thar returns an instance of BigBased constructed from a string
        /// that is matching <see cref="validStringRegEx"/> 
        /// and is cut with <see cref="cleanStringRegEx"/>
        /// of base bigBase</summary>
        /// <param name="str">String that represents a number</param>
        /// <param name="bigBase">A 2..16 base of number</param>
        /// <returns>An instance of BigBased. null if any parameter is invalid</returns>
        /// <exception cref="ArgumentException">Exception thrown if
        /// bigBase parameter is not in range 2..16</exception>
        public static BigBased CreateFromString(string str, int bigBase)
        {
            if (bigBase < 2 || bigBase > 16)
                throw new ArgumentException("BigBased base must fit 2..16 range. Actual value: " + bigBase);
            if (string.IsNullOrEmpty(str))
                return new BigBased(bigBase);

            string regExReplacer;
            string validStringRegEx, cleanStringRegEx;
            if (bigBase <= 10)
                regExReplacer = (bigBase - 1).ToString();
            else
                regExReplacer = "9a-"
                    + ToChar(bigBase - 1)
                    + "A-"
                    + ToChar(bigBase - 1, true);

            validStringRegEx = String.Format(validRegexFormat, regExReplacer);
            cleanStringRegEx = String.Format(cleanRegexFormat, regExReplacer);

            if (Regex.IsMatch(str, validStringRegEx, RegexOptions.None))
                return new BigBased();
            str = CleanNumericString(str, cleanStringRegEx, out int sign);
            return new BigBased(str, sign, bigBase);
        }
        /// <summary>Fabric thar returns an instance of BigBased constructed from a number</summary>
        /// <param name="number">Object of any numeric type</param>
        /// <param name="bigBase">Desired base of number</param>
        /// <returns>An instance of BigBased. null if parameter is invalid</returns>
        public static BigBased CreateFromNumber<T>(T number, int bigBase)
        {
            //todo: this is incorrect because not converting bases
            string str = number.ToString();
            throw new NotImplementedException("CreateFromNumber<T> not implemented yet");
        }
        /// <summary>CleanNumericString method cleans digit string with <see cref="cleanStringRegEx"/></summary>
        /// <param name="RawString">String representing of digits</param>
        /// <param name="cleanStringRegEx">RegEx used to clean the string</param>
        /// <param name="sign">Integer representing position of dot in cleaned string</param>
        /// <returns>A clean string; "0" in case of invalid arguments</returns>
        private static string CleanNumericString(string RawString, string cleanStringRegEx, out int sign)
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
        /// <summary>IntListToString method converts digit list to string</summary>
        /// <param name="digits">List of digits</param>
        /// <param name="dotPos">Integer representing reversed position of dot in string</param>
        /// <returns>A string representing a number; null in case of invalid arguments</returns>
        public static string IntListToString(List<int> digits)
        {
            int i;
            StringBuilder sb;

            if (digits is null || digits.Count == 0)
                return "";
            sb = new StringBuilder();
            for (i = digits.Count - 1; i >= 0; i--)
                sb.Append(ToChar(digits[i]));
            return sb.ToString();
        }
        /// <summary>Сonverts BigBased into reversed digit list with padding zeroes</summary>
          /// <param name="desiredInt">int represening how many digits should integer part contain</param>
          /// <param name="desiredFrac">int represening how many digits should fractional part contain</param>
          /// <returns>List of digits</returns>
        public static List<int> BigBasedToIntList(BigBased num, int desiredInt = 0)
        {
            if (num is null)
                return null;

            List<int> ret = new List<int>();
            int IntZeros;

            IntZeros = Math.Max(num.CleanString.Length, desiredInt) - num.CleanString.Length;
            for (int i = num.CleanString.Length - 1; i >= 0; i--)
                ret.Add(ToDigit(num.CleanString[i]));
            ret.AddRange(Enumerable.Repeat(0, IntZeros));
            return ret;
        }

        /// <summary>Converts numbers into matching char symbols</summary>
        /// <param name="digit">Number to be converted. Must be in [0..15] range</param>
        /// <param name="toUpper">shows if result should be uppercase</param>
        /// <returns>A matching char; '0' if digit does not match limits</returns>
        public static char ToChar(int digit, bool toUpper = false)
        {
            if (digit >= 0 && digit < 10)
                return digit.ToString()[0];
            if (digit > 9 && digit < 16)
            {
                if (!toUpper)
                    return Convert.ToChar('a' + digit - 10);
                return Convert.ToChar('A' + digit - 10);
            }
            return '0';
        }
        /// <summary>Converts characters to matching number</summary>
        /// <param name="c">Character to be converted. Must consist of [0..9a..dA..D]</param>
        /// <returns>A matching number; -1 if parameter does not match limits.</returns>
        public static int ToDigit(char c)
        {
            if (Char.IsDigit(c))
                return Convert.ToInt32(c - '0');
            if (char.IsLetter(c))
            {
                c = Char.ToLower(c);
                if (c < 'g')
                    return c - 'a' + 10;
            }
            return -1;
        }
        #endregion

        #region Public tools
        /// <summary>Returns a copy of BigBased instance </summary>
        /// <returns>Copy of BigBased instance</returns>
        public BigBased Copy()
        {
            return new BigBased(this);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            return (this == obj as BigBased);
        }
        /// <summary> Returns |BigBased| </summary>
        /// <param name="bf">BigBased parameter</param>
        /// <returns>|BigBased|</returns>
        public static BigBased Abs(BigBased bf)
        {
            BigBased ret = new BigBased(bf);
            if (bf.Sign < 0)
                bf.Negate();
            return bf;
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
                    digits[i] += Base;
                    digits[i + 1]--;
                }
                else
                {
                    digits[i + 1] += digits[i] / Base;
                    digits[i] %= Base;
                }
            }
            while (digits[i] >= Base)
            {
                digits.Add(digits[i] / Base);
                digits[i] %= Base;
                i++;
            }
        }

        /// <summary>Method that is calculating (this + op) for BigBased objects.
        /// Overrided from parent</summary>
        /// <param name="op">Second operand</param>
        /// <returns>BigBased equal to (this + op)  upcasted to BigNumber</returns>
        public override BigNumber Add(BigNumber op)
        {
            if (!(op is BigBased))
                throw new ArgumentException("Cannot Add BigBased and " + op.GetType());

            BigBased bfLeft = this;
            BigBased bfRight = (BigBased)op;

            if (bfLeft.Base != bfRight.Base)
                throw new NotImplementedException
                    ("Operations for different-based numbers is not implemented yet");

            if (bfLeft.Sign != bfRight.Sign)
                return bfLeft.Substract(-bfRight);

            int desiredInt = Math.Max(bfLeft.CleanString.Length, bfRight.CleanString.Length);
            var leftList = BigBasedToIntList(bfLeft, desiredInt);
            var rightList = BigBasedToIntList(bfRight, desiredInt);
            var resultList = leftList.SumWithList(rightList);
            NormalizeList(resultList);

            BigBased bfAns = CreateFromString(IntListToString(resultList), bfLeft.Base);
            if (Sign < 0)
                bfAns.Negate();
            return bfAns;
        }
        /// <summary>Method that is calculating (this - op) for BigBased objects.
        /// Overrided from parent</summary>
        /// <param name="op">Second operand</param>
        /// <returns>BigBased equal to (this - op)  upcasted to BigNumber</returns>
        public override BigNumber Substract(BigNumber op)
        {
            if (!(op is BigBased))
                throw new ArgumentException("Cannot Add BigBased and " + op.GetType());

            BigBased bfLeft = this;
            BigBased bfRight = (BigBased)op;

            if (bfLeft.Base != bfRight.Base)
                throw new NotImplementedException
                    ("Operations for different-based numbers is not implemented yet");

            if (bfLeft.Sign > 0 && bfRight.Sign < 0)
                return bfLeft.Add(-bfRight);
            if (bfLeft.Sign < 0 && bfRight.Sign > 0)
                return -(BigBased)bfRight.Add(-bfLeft);
            if (bfLeft.Sign < 0 && bfRight.Sign < 0)
                return (-bfRight).Substract(-bfLeft);
            //both operands are > 0 here
            int sign = 1;

            if (bfLeft < bfRight)
            {
                sign = -sign;
                Swap(ref bfLeft, ref bfRight);
            }
            int desiredInt = Math.Max(bfLeft.CleanString.Length, bfRight.CleanString.Length);
            var leftList = BigBasedToIntList(bfLeft, desiredInt);
            var rightList = BigBasedToIntList(bfRight, desiredInt);
            var resultList = leftList.SubByList(rightList);
            NormalizeList(resultList);

            BigBased bfAns = CreateFromString(IntListToString(resultList), bfLeft.Base);
            if (sign < 0)
                bfAns.Negate();
            return bfAns;
        }
        /// <summary>Method that is calculating (this * op) for BigBased objects.
        /// Overrided from parent</summary>
        /// <param name="op">Second operand</param>
        /// <returns>BigBased equal to (this * op)  upcasted to BigNumber</returns>
        public override BigNumber Multiply(BigNumber op)
        {
            if (!(op is BigBased))
                throw new ArgumentException("Cannot Add BigBased and " + op.GetType());

            BigBased bfLeft = this;
            BigBased bfRight = (BigBased)op;

            if (bfLeft.Base != bfRight.Base)
                throw new NotImplementedException
                    ("Operations for different-based numbers is not implemented yet");

            if (bfLeft.CleanString.Length < bfRight.CleanString.Length)
                Swap(ref bfLeft, ref bfRight);
            var leftList = BigBasedToIntList(bfLeft);
            var rightList = BigBasedToIntList(bfRight);
            var resultList = leftList.MulWithList(rightList);
            NormalizeList(resultList);

            BigBased bfAns = CreateFromString(IntListToString(resultList), bfLeft.Base);
            if (bfLeft.Sign * bfRight.Sign < 0)
                bfAns.Negate();
            return bfAns;
        }
        /// <summary>Method that is calculating (this / op) for BigBased objects.
        /// Overrided from parent</summary>
        /// <param name="op">Second operand</param>
        /// <returns>BigBased equal to (this / op)  upcasted to BigNumber</returns>
        public override BigNumber Divide(BigNumber op)
        {
            if (!(op is BigBased))
                throw new ArgumentException("Cannot Add BigBased to " + op.GetType());

            if (op.CleanString == "0")
                throw new DivideByZeroException();
            BigBased bfLeft = this;
            BigBased bfRight = (BigBased)op;

            if (bfLeft.Base != bfRight.Base)
                throw new NotImplementedException
                    ("Operations for different-based numbers is not implemented yet");

            var leftList = BigBasedToIntList(bfLeft, 0);
            var rightList = BigBasedToIntList(bfRight, 0);
            leftList.RemoveTailingZeros();
            rightList.RemoveTailingZeros();

            List<int> resultList = leftList.DivByList(rightList, NormalizeList, out List<int> subList);
            BigBased bfAns = CreateFromString(IntListToString(resultList), bfLeft.Base);
            if (bfLeft.Sign * bfRight.Sign < 0)
                bfAns.Negate();
            return bfAns;
        }
        /// <summary>Method that is calculating (this % op) for BigBased objects.
        /// Overrided from parent</summary>
        /// <param name="op">Second operand</param>
        /// <returns>BigBased equal to (this % op)  upcasted to BigNumber</returns>
        public override BigNumber Mod(BigNumber op)
        {
            if (!(op is BigBased))
                throw new ArgumentException("Cannot Add BigBased and " + op.GetType());

            if (op.CleanString == "0")
                throw new ArgumentException("Cannot calculate BigBased % 0");
            BigBased bfLeft = this;
            BigBased bfRight = (BigBased)op;

            if (bfLeft.Base != bfRight.Base)
                throw new NotImplementedException
                    ("Operations for different-based numbers is not implemented yet");

            BigBased bfDiv = bfLeft / bfRight;
            BigBased bfAns = bfLeft - bfDiv * bfRight;
            return bfAns;
        }
        /// <summary>Indexer allowing to get indexed digit values</summary>
        /// <param name="index">Integer representing index</param>
        /// <returns>Integer representing digit</rturns>
        public override int this[int index]
        {
            get
            {
                if (index < 0 || index >= CleanString.Length)
                    return -1;
                return ToDigit(CleanString[index]);
            }
        }
        #endregion

        #region Operators
        public static BigBased operator -(BigBased num)
        {
            BigBased ret = new BigBased(num);

            ret.Negate();
            return ret;
        }

        public static BigBased operator +(BigBased left, BigBased right)
        {
            if (left is null || right is null)
                return null;
            return (BigBased)left.Add(right);
        }
        public static BigBased operator -(BigBased left, BigBased right)
        {
            if (left is null || right is null)
                return null;
            return (BigBased)left.Substract(right);
        }
        public static BigBased operator *(BigBased left, BigBased right)
        {
            if (left is null || right is null)
                return null;
            return (BigBased)left.Multiply(right);
        }
        public static BigBased operator /(BigBased left, BigBased right)
        {
            if (left is null || right is null)
                return null;
            return (BigBased)left.Divide(right);
        }
        public static BigBased operator %(BigBased left, BigBased right)
        {
            if (left is null || right is null)
                return null;
            return (BigBased)left.Mod(right);
        }

        public static bool operator >(BigBased left, BigBased right)
        {
            if (left.Base != right.Base)
                throw new NotImplementedException
                    ("Operations for different-based numbers is not implemented yet");

            if (left.Sign > 0)
            {
                if (right.Sign < 0)
                    return true;
                if (left.CleanString.Length > right.CleanString.Length)
                    return true;
                if (left.CleanString.Length < right.CleanString.Length)
                    return false;
                if (string.Compare(left.CleanString, right.CleanString) > 0)
                    return true;
                return false;
            }
            if (right.Sign > 0)
                return false;
            if (left.CleanString.Length > right.CleanString.Length)
                return false;
            if (left.CleanString.Length < right.CleanString.Length)
                return true;
            if (string.Compare(left.CleanString, right.CleanString) > 0)
                return false;
            return true;
        }
        public static bool operator <(BigBased left, BigBased right)
        {
            return (!(left > right));
        }
        public static bool operator ==(BigBased left, BigBased right)
        {
            if (left.Base != right.Base)
                throw new NotImplementedException
                    ("Operations for different-based numbers is not implemented yet");

            if (string.Compare(left.CleanString, right.CleanString) != 0 
                || left.Sign != right.Sign)
                return false;
            return true;
        }
        public static bool operator !=(BigBased left, BigBased right)
        {
            if (left.Base != right.Base)
                throw new NotImplementedException
                    ("Operations for different-based numbers is not implemented yet");

            if (string.Compare(left.ToString(), right.ToString()) != 0)
                return false;
            return true;
        }
        #endregion

        #region Variables
        /// <summary>validRegexFormat is a string representing RegEx format
        /// used to validate input string in fabric method <see cref="new BigInteger"/>
        /// into integer and fractional parts </summary>
        private static readonly string validRegexFormat = @"^\s*[+-]?[0-{0}]+\s*$";
        /// <summary>cleanRegexFormat is a string representing RegEx format
        /// used to clean valid input string in fabric method <see cref="new BigInteger"/></summary>
        private static readonly string cleanRegexFormat = @"[1-{0}]+[0-{0}]*";
        #endregion

        #region Properties
        public int Base { get; private set; } = 10;
        #endregion
    }
}
