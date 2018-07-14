using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

namespace BignumArithmetics
{
    /// <summary>Сlass for big numbers having integer and fractional parts</summary>
    public class BigFloat : BigNumber
    {
        #region Constructors
        public BigFloat()
        {
            CleanString = "0";
        }
        public BigFloat(BigFloat from)
        {
            CleanString = from.CleanString;
            if (from.Sign < 0)
                SwitchSign();
            DotPos = from.DotPos;
            Fractional = from.Fractional;
        }
        private BigFloat(string str, int sign)
        {
            CleanString = str;
            if (sign < 0)
                SwitchSign();
        }
        #endregion

        #region Static Methods
        /// <summary>Fabric thar returns an instance of BigFloat created of string</summary>
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
        /// <summary>Fabric thar returns an instance of BigFloat created of number</summary>
        /// <param name="number">Object of any numeric type</param>
        /// <returns>An instance of BigFloat. null if parameter is invalid</returns>
        public static BigFloat CreateFromNumber<T>(T number)
        {
            string str = number.ToString();
            string sysDelim = System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            str = str.Replace(sysDelim, delimiter);
            return CreateFromString(str);
        }
        /// <summary>Сonverts BigNum into reversed digit list with padding zeroes</summary>
        /// <param name="desiredInt">int represening how many digits should integer part contain</param>
        /// <param name="desiredFrac">int represening how many digits should fractional part contain</param>
        /// <returns>List of digits</returns>
        public static List<int> BigFloatToIntList(BigFloat num, int desiredInt = 0, int desiredFrac = 0)
        {
            List<int> ret = new List<int>();
            int IntZeros, FracZeros;

            if (num is null)
                return null;
            IntZeros = Math.Max(num.Integer, desiredInt) - num.Integer;
            FracZeros = Math.Max(num.Fractional, desiredFrac) - num.Fractional;
            ret.AddRange(Enumerable.Repeat(0, FracZeros));
            for (int i = num.CleanString.Length - 1; i >= 0; i--)
                if (num.CleanString[i] != '.')
                    ret.Add(ToDigit(num.CleanString[i]));
            ret.AddRange(Enumerable.Repeat(0, IntZeros));
            return ret;
        }
        /// <summary>Normalizes reversed list of digits</summary>
        /// <param name="digits">List of digits</param>
        public static void NormalizeList(List<int> digits)
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
        /// <summary>Converts digit list to string</summary>
        /// <param name="digits">List of digits</param>
        /// <param name="dotPos">Integer representing position of dot in string</param>
        /// <returns>A string representing a number; null in case of invalid arguments</returns>
        public static string IntListToString(List<int> digits, int dotPos)
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

        private static string CleanNumericString(string RawString, out int sign)
        {
            string substr;

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

        #region Private Methods
        private void FindDotPos()
        {
            if (_dotPos > 0)
                return;
            _dotPos = CleanString.IndexOf(delimiter);
            if (_dotPos < 0)
                _dotPos = CleanString.Length;
        }
        private List<int> SumTwoLists(List<int> leftList, List<int> rightList)
        {
            if (leftList.Count <= 0 || rightList.Count <= 0)
                return new List<int>();
            int diff = Math.Max(leftList.Count, rightList.Count);
            leftList.AddRange(Enumerable.Repeat(0, diff - leftList.Count));
            rightList.AddRange(Enumerable.Repeat(0, diff - rightList.Count));
            var resultList = new List<int>(leftList.Count);
            for (int i = 0; i < diff; i++)
                resultList.Add(leftList[i] + rightList[i]);
            NormalizeList(resultList);
            return resultList;
        }
        private List<int> DifTwoLists(List<int> leftList, List<int> rightList)
        {
            int until = Math.Min(leftList.Count, rightList.Count);
            if (until <= 0)
                return new List<int>();
            var resultList = new List<int>(until);
            for (int i = 0; i < until; i++)
                resultList.Add(leftList[i] - rightList[i]);
            NormalizeList(resultList);
            return resultList;
        }
        private List<int> MulTwoLists(List<int> leftList, List<int> rightList)
        {
            var resultList = new List<int>();
            var tempList = new List<int>();
            resultList = MulListAndDigit(leftList, rightList[0], false);
            for (int i = 1; i < rightList.Count; i++)
            {
                if (rightList[i] == 0)
                    continue;
                tempList = MulListAndDigit(leftList, rightList[i], false, i);
                resultList = SumTwoLists(resultList, tempList);
            }
            NormalizeList(resultList);
            return resultList;
        }

        private List<int> MulListAndDigit(List<int> leftList, int digit, bool toNorm = false, int padding = 0)
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
        #endregion

        #region Parent Overrides
        public override BigNumber Sum(BigNumber op)
        {
            BigFloat bfLeft = this;
            BigFloat bfRight = (BigFloat)op;

            if (bfLeft.Sign != bfRight.Sign)
                return bfLeft.Dif(-bfRight);

            int desiredInt = Math.Max(bfLeft.Integer, bfRight.Integer);
            int desiredFrac = Math.Max(bfLeft.Fractional, bfRight.Fractional);
            var leftList = BigFloatToIntList(bfLeft, desiredInt, desiredFrac);
            var rightList = BigFloatToIntList(bfRight, desiredInt, desiredFrac);
            var resultList = SumTwoLists(leftList, rightList);

            BigFloat bfAns = CreateFromString(IntListToString(resultList, resultList.Count - desiredFrac));
            if (Sign < 0)
                bfAns.SwitchSign();
            return bfAns;
        }
        public override BigNumber Dif(BigNumber op)
        {
            BigFloat bfLeft = this;
            BigFloat bfRight = (BigFloat)op;

            if (bfLeft.Sign > 0 && bfRight.Sign < 0)
                return bfLeft.Sum(-bfRight);
            if (bfLeft.Sign < 0 && bfRight.Sign > 0)
                return -(BigFloat)bfRight.Sum(-bfLeft);
            if (bfLeft.Sign < 0 && bfRight.Sign < 0)
                return (-bfRight).Dif(-bfLeft);
            //both operands are > 0 here
            int sign = 1;

            if (bfLeft < bfRight)
            {
                sign = -sign;
                Swap(ref bfLeft, ref bfRight);
            }
            int desiredInt = Math.Max(bfLeft.Integer, bfRight.Integer);
            int desiredFrac = Math.Max(bfLeft.Fractional, bfRight.Fractional);
            var leftList = BigFloatToIntList(bfLeft, desiredInt, desiredFrac);
            var rightList = BigFloatToIntList(bfRight, desiredInt, desiredFrac);
            var resultList = DifTwoLists(leftList, rightList);
        
            BigFloat bfAns = CreateFromString(IntListToString(resultList, resultList.Count - desiredFrac));
            if (sign < 0)
                bfAns.SwitchSign();
            return bfAns;
        }
        public override BigNumber Mul(BigNumber op)
        {
            //12.4 = 1256  \ (10^1)
            //0.25 = 025 \ (10^2)
            //12.4 * 0.25 = 124 * 25 \ (10^3) = (2500 + 500 + 100) \ (10^3) = 3100 \ 1000 = 3.1

            BigFloat bfLeft = this;
            BigFloat bfRight = (BigFloat)op;

            //mul bigger number with smaller digits
            if (bfLeft.Integer + bfLeft.Fractional < bfRight.Integer + bfRight.Fractional)
                Swap(ref bfLeft, ref bfRight);
            int newDot = bfLeft.Fractional + bfRight.Fractional;
            var leftList = BigFloatToIntList(bfLeft);
            var rightList = BigFloatToIntList(bfRight);
            var resultList = MulTwoLists(leftList, rightList);

            BigFloat bfAns = CreateFromString(IntListToString(resultList, resultList.Count - newDot));
            if (bfLeft.Sign * bfRight.Sign < 0)
                bfAns.SwitchSign();
            return bfAns;
        }
        public override BigNumber Div(BigNumber op)
        {
            return new BigFloat();
        }
        #endregion

        #region Operators
        public static BigFloat operator -(BigFloat num)
        {
            BigFloat ret = new BigFloat(num);

            ret.SwitchSign();
            return ret;
        }

        public static BigFloat operator +(BigFloat left, BigFloat right)
        {
            if (left is null || right is null)
                return null;
            return (BigFloat)left.Sum(right);
        }
        public static BigFloat operator -(BigFloat left, BigFloat right)
        {
            if (left is null || right is null)
                return null;
            return (BigFloat)left.Dif(right);
        }
        public static BigFloat operator *(BigFloat left, BigFloat right)
        {
            if (left is null || right is null)
                return null;
            return (BigFloat)left.Mul(right);
        }
        public static BigFloat operator /(BigFloat left, BigFloat right)
        {
            if (left is null || right is null)
                return null;
            return (BigFloat)left.Div(right);
        }

        public static bool operator >(BigFloat left, BigFloat right)
        {
            if (left.Integer > right.Integer)
                return true;
            if (left.Integer < right.Integer)
                return false;
            if (string.Compare(left.CleanString, right.CleanString) > 0)
                return true;
            return false;
        }
        public static bool operator <(BigFloat left, BigFloat right)
        {
            if (left.Integer > right.Integer)
                return false;
            if (left.Integer < right.Integer)
                return true;
            if (string.Compare(left.CleanString, right.CleanString) < 0)
                return true;
            return false;
        }
        #endregion

        #region Variables
        private static readonly string delimiter = ".";
        private static readonly string validStringRegEx = @"^\s*[+-]?[0-9]+(\.[0-9]+)?\s*$";
        private static readonly string cleanStringRegEx = @"([1-9]+[0-9]*(\.[0-9]*[1-9]+)?|0\.[0-9]*[1-9]+)";

        private volatile int _dotPos = 0;
        private volatile int _fracLen = -1;
        #endregion

        #region Mutexes
        private readonly Object dotPosMutex = new Object();
        private readonly Object fracLenMutex = new Object();
        #endregion

        #region Properties
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
        public int Integer
        {
            get
            {
                return DotPos;
            }
        }
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
        #endregion
    }
}
