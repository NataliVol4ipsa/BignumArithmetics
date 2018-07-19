using System;
using System.Collections.Generic;
using System.Linq;

//todo: force children to overload operators

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

        #region Calculations Methods
        /// <summary> Generates a list representing sum of two reversed digit lists</summary>
        /// <param name="leftList">First operand</param>
        /// <param name="rightList">Second operand</param>
        /// <param name="toNorm">Shows if the <see cref="NormalizeList(List{int})"/>
        /// should be called for result</param>
        /// <returns>List representing sum of two lists in same reversed form</returns>
        protected List<int> SumTwoLists(List<int> leftList, List<int> rightList, bool toNorm = true)
        {
            if (leftList.Count <= 0 || rightList.Count <= 0)
                return new List<int>();
            int maxlen = Math.Max(leftList.Count, rightList.Count);
            leftList.AddRange(Enumerable.Repeat(0, maxlen - leftList.Count));
            rightList.AddRange(Enumerable.Repeat(0, maxlen - rightList.Count));
            var resultList = new List<int>(leftList.Count);
            for (int i = 0; i < maxlen; i++)
                resultList.Add(leftList[i] + rightList[i]);
            if (toNorm)
                NormalizeList(resultList);
            return resultList;
        }
        /// <summary> Generates a list representing substraction of two reversed digit lists.</summary>
        /// <param name="leftList">First list.
        /// Number represented by leftList has to be bigger than the one by rightList!</param>
        /// <param name="rightList">Second list.</param>
        /// <param name="toNorm">Shows if the <see cref="NormalizeList(List{int})"/>
        /// should be called for result</param>
        /// <returns>List representing substraction of two lists in same reversed form</returns>
        /// <exception cref="ArgumentException">Exception thrown if leftList < rightList</exception>
        protected List<int> DifTwoLists(List<int> leftList, List<int> rightList, bool toNorm = true)
        {
            if (CompareLists(leftList, rightList) < 0)
                throw new ArgumentException("leftList cannot be bigger than rightList!");
            rightList.AddRange(Enumerable.Repeat(0, leftList.Count - rightList.Count));
            var resultList = new List<int>();
            for (int i = 0; i < leftList.Count; i++)
                resultList.Add(leftList[i] - rightList[i]);
            if (toNorm)
                NormalizeList(resultList);
            return resultList;
        }
        /// <summary> Generates a list representing multiplication of two reversed digit lists.</summary>
        /// <param name="leftList">First list.</param>
        /// <param name="rightList">Second list.</param>
        /// <param name="toNorm">Shows if the <see cref="NormalizeList(List{int})"/>
        /// should be called for result</param>
        /// <returns>List representing multiplication of two lists in same reversed form</returns>
        protected List<int> MulTwoLists(List<int> leftList, List<int> rightList, bool toNorm = false)
        {
            var resultList = new List<int>();
            var tempList = new List<int>();
            resultList = MulListAndDigit(leftList, rightList[0], false);
            for (int i = 1; i < rightList.Count; i++)
            {
                if (rightList[i] == 0)
                    continue;
                tempList = MulListAndDigit(leftList, rightList[i], false, i);
                resultList = SumTwoLists(resultList, tempList, false);
            }
            if (toNorm)
                NormalizeList(resultList);
            return resultList;
        }
        /// <summary> Generates a list representing division of two reversed digit lists.</summary>
        /// <param name="leftList">First list.</param>
        /// <param name="rightList">Second list.</param>
        /// <param name="remainder">A list equal to leftList mod rightList"/>
        /// <returns>List representing division of two lists in same reversed form</returns>
        protected List<int> DivTwoLists(List<int> leftList, List<int> rightList, out List<int> remainder)
        {
            var resultList = new List<int>();
            remainder = new List<int>();
            if (CompareLists(leftList, rightList) >= 0)
            {
                bool unnormed = true;
                int sum, dif;
                int indexToAdd = leftList.Count - rightList.Count - 1;
                remainder.AddRange(leftList.GetRange(indexToAdd + 1, rightList.Count));
                do
                {
                    if (unnormed)
                        while (remainder.Count > 0 && remainder.Last() == 0)
                            remainder.RemoveAt(remainder.Count - 1);
                    unnormed = true;
                    sum = 0;
                    while (CompareLists(remainder, rightList) >= 0)
                    {
                        dif = remainder.Count - rightList.Count;
                        rightList.AddRange(Enumerable.Repeat(0, dif));
                        remainder = DifTwoLists(remainder, rightList);
                        if (dif > 0)
                            rightList.RemoveAt(rightList.Count - 1);
                        while (remainder.Count > 0 && remainder.Last() == 0)
                            remainder.RemoveAt(remainder.Count - 1);
                        sum++;
                        unnormed = false;
                    }
                    resultList.Add(sum);
                    if (indexToAdd >= 0)
                        remainder.Insert(0, leftList[indexToAdd]);
                    indexToAdd--;
                } while (indexToAdd >= -1);
            }
            if (resultList.Count == 0)
                resultList.Add(0);
            resultList.Reverse();
            return resultList;
        }
        /// <summary> Generates a list representing multiplication of reversed digit lists and digit</summary>
        /// <param name="leftList">A list to be multiplicated</param>
        /// <param name="digit">A digit to be multiplicated</param>
        /// <param name="toNorm">Shows if the <see cref="NormalizeList(List{int})"/>
        /// should be called for result</param>
        /// <param name="padding">Optional parameter representing
        /// number of zeroes to be padded at the beginning of result list</param>
        /// <returns>List representing multiplication of reversed digit lists and digit</returns>
        protected List<int> MulListAndDigit(List<int> leftList, int digit, bool toNorm = true, int padding = 0)
        {
            if (digit == 0)
                return new List<int> { 0 };
            var resultList = new List<int>(leftList.Count);
            resultList.AddRange(Enumerable.Repeat(0, padding));
            for (int i = 0; i < leftList.Count; i++)
                resultList.Add(leftList[i] * digit);
            if (toNorm)
                NormalizeList(resultList);
            return resultList;
        }

        /// <summary> Compares two reversed list of digits 
        /// and returns an int representing their order when sorted </summary>
        /// <param name="left">First list to compare</param>
        /// <param name="right">Second list to compare</param>
        /// <returns>An int representing list order when sorted</returns>
        protected int CompareLists(List<int> left, List<int> right)
        {
            if (left.Count != right.Count)
                return left.Count - right.Count;
            int i = left.Count - 1;
            while (i >= 0 && left[i] == right[i])
                i--;
            if (i == -1)
                return 0;
            return left[i] - right[i];
        }
        /// <summary>Removes extra tailing zeroes from digit list</summary>
        /// <param name="list">List to be cleaned</param>
        protected void RemoveTailingZeros(List<int> list)
        {
            while (list.Last() == 0 && list.Count > 1)
                list.RemoveAt(list.Count - 1);
        }
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
        public void Negate()
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
