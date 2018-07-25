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

        #region Static Methods
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
        /// <returns>Integer representing digit</rturns>
        public abstract int this[int index] { get; }
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
