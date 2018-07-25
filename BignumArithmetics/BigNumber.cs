using System;
using System.Collections.Generic;
using System.Linq;

namespace BignumArithmetics
{
    /// <summary>Abstract class for big numbers</summary>
    public abstract class BigNumber
    {
        #region Abstract Methods
        /// <summary>Normalizes reversed list of digits</summary>
        /// <param name="digits">List of digits</param>
        public abstract void NormalizeList(List<int> digits);
        /// <summary>Abstract method that is calculating (this + op).
        /// Has to be overrided by children</summary>
        /// <param name="op">Second operand</param>
        /// <returns>BigNumber equal to (this + op)</returns>
        public abstract BigNumber Add(BigNumber op);
        /// <summary>Abstract method that is calculating (this - op).
        /// Has to be overrided by children</summary>
        /// <param name="op">Second operand</param>
        /// <returns>BigNumber equal to (this - op)</returns>
        public abstract BigNumber Substract(BigNumber op);
        /// <summary>Abstract method that is calculating (this * op).
        /// Has to be overrided by children</summary>
        /// <param name="op">Second operand</param>
        /// <returns>BigNumber equal to (this * op)</returns>
        public abstract BigNumber Multiply(BigNumber op);
        /// <summary>Abstract method that is calculating (this / op).
        /// Has to be overrided by children</summary>
        /// <param name="op">Second operand</param>
        /// <returns>BigNumber equal to (this / op)</returns>
        public abstract BigNumber Divide(BigNumber op);
        /// <summary>Abstract method that is calculating (this % op).
        /// Has to be overrided by children</summary>
        /// <param name="op">Second operand</param>
        /// <returns>BigNumber equal to (this % op)</returns>
        public abstract BigNumber Mod(BigNumber op);
        #endregion

        //TODO: MOVE TO CHILDREN
        #region Static Methods
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
        /// <summary> Negate is switching <see cref="Sign"/> to the opposite one </summary>
        public void Negate()
        {
            if (String.Compare(CleanString, "0") != 0)
                Sign = -Sign;
            else
                Sign = 1;
        }
        /// <summary>Indexer allowing to get indexed digit values</summary>
        /// <param name="index">Integer representing index</param>
        /// <returns>Integer representing digit</returns>
        public virtual int this[int index]
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
        public static BigNumber operator +(BigNumber left, BigNumber right)
        {
            if (left is null || right is null)
                return null;
            return left.Add(right);
        }
        public static BigNumber operator -(BigNumber left, BigNumber right)
        {
            if (left is null || right is null)
                return null;
            return left.Substract(right);
        }
        public static BigNumber operator *(BigNumber left, BigNumber right)
        {
            if (left is null || right is null)
                return null;
            return left.Multiply(right);
        }
        public static BigNumber operator /(BigNumber left, BigNumber right)
        {
            if (left is null || right is null)
                return null;
            return left.Divide(right);
        }
        public static BigNumber operator %(BigNumber left, BigNumber right)
        {
            if (left is null || right is null)
                return null;
            return left.Mod(right);
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
