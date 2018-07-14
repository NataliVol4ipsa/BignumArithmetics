using System;
using System.Text.RegularExpressions;

//todo: force children to overload operators

namespace BignumArithmetics
{
    /// <summary>Abstract class for big numbers</summary>
    public abstract class BigNumber
    {
        #region Private Methods
        public abstract BigNumber Sum(BigNumber op);
        public abstract BigNumber Dif(BigNumber op);
        public abstract BigNumber Mul(BigNumber op);
        public abstract BigNumber Div(BigNumber op);
        #endregion

        #region Static Methods
        /// <summary>Converts numbers into matching char symbols</summary>
        /// <param name="digit">Number to be converted. Must be in [0..15] range</param>
        /// <returns>A matching char; '0' if digit does not match limits</returns>
        public static char ToChar(int digit)
        {
            if (digit >= 0 && digit < 10)
                return digit.ToString()[0];
            if (digit > 9 && digit < 16)
                return Convert.ToChar('a' + digit - 10);
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
        /// <summary>Swaps two objects of type T</summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="left">First parameter to swap</param>
        /// <param name="right">Second parameter to swap</param>
        public static void Swap<T>(ref T left, ref T right)
        {
            T buf = left;
            left = right;
            right = buf;
        }
        #endregion

        #region Public Methods
        public void SwitchSign()
        {
            if (String.Compare(CleanString, "0") != 0)
                Sign = -Sign;
            else
                Sign = 1;
        }
        #endregion

        #region Operators
        public static BigNumber operator +(BigNumber left, BigNumber right)
        {
            if (left is null || right is null)
                return null;
            return left.Sum(right);
        }
        public static BigNumber operator -(BigNumber left, BigNumber right)
        {
            if (left is null || right is null)
                return null;
            return left.Dif(right);
        }
        public static BigNumber operator *(BigNumber left, BigNumber right)
        {
            if (left is null || right is null)
                return null;
            return left.Mul(right);
        }
        public static BigNumber operator /(BigNumber left, BigNumber right)
        {
            if (left is null || right is null)
                return null;
            return left.Div(right);
        }
        #endregion

        #region Overloading
        public override string ToString()
        {
            return (Sign > 0 ? CleanString : "-" + CleanString);
        }
        #endregion

        #region Properties
        /// <summary>The CleanString property represents number without spaces, extra zeroes etc</summary>
        /// <value> The CleanString property gets/sets the value of the string field, _cleanString</value>
        public string CleanString { get; protected set; } = "0";
        /// <summary>The Sign property represents sign of the number</summary>
        /// <value> The CleanString property gets/sets the value of the int field, _sign</value>
        public int Sign { get; private set; } = 1;
        #endregion
    }
}
