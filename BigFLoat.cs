using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

//todo: add constructor taking numeric types as parameter

namespace BignumArithmetics
{
    public class BigFloat : BigNumber
    {
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

        /* === Static Methods === */
        /// <summary>Fabric thar returns an instance of BigFloat created of string</summary>
        /// <param name="str">String that represents a number</param>
        /// <returns>An instance of BigFloat. null if parameter is invalid</returns>
        public static BigFloat CreateFromString(string str)
        {
            if (str is null ||
                !Regex.IsMatch(str, validStringRegEx, RegexOptions.None))
                return new BigFloat();
            str = CleanNumericString(str, out int sign);
            return new BigFloat(str, sign);
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

        /// <summary>Сonverts BigNum into reversed digit list</summary>
        /// <param name="desiredInt">int represening how many digits should integer part contain</param>
        /// <param name="desiredFrac">int represening how many digits should fractional part contain</param>
        /// <returns>List of digits</returns>
        public static List<int> BigFloatToIntList(BigFloat num, int desiredInt, int desiredFrac)
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
            StringBuilder sb = new StringBuilder();

            if (digits is null || digits.Count == 0)
                return "";
            if (dotPos < 0)
                dotPos = 0;
            reverseDot = digits.Count - dotPos;
            for (i = digits.Count - 1; i >= reverseDot; i--)
                sb.Append(ToChar(digits[i]));
            if (i >= 0)
            {
                sb.Append(".");
                while (i >= 0)
                    sb.Append(ToChar(digits[i--]));
            }
            return sb.ToString();
        }

        /* === Private Methods === */
        private void FindDotPos()
        {
            if (_dotPos > 0)
                return;
            _dotPos = CleanString.IndexOf(".");
            if (_dotPos < 0)
                _dotPos = CleanString.Length;
        }

        /* === Parent Overrides === */
        //no sign support
        public override BigNumber Sum(BigNumber op)
        {
            BigFloat bfLeft = this;
            BigFloat bfRight = (BigFloat)op;
            BigFloat bfAns;

            //5 + -6 = 5 - -(-6) = 5 - 6
            if (bfLeft.Sign != bfRight.Sign)
                return bfLeft.Dif(-bfRight);
            
            int desiredInt = Math.Max(Integer, bfRight.Integer);
            int desiredFrac = Math.Max(Fractional, bfRight.Fractional);
            var leftList = BigFloatToIntList(this, desiredInt, desiredFrac);
            var rightList = BigFloatToIntList(bfRight, desiredInt, desiredFrac);
            var resultList = new List<int>(leftList.Count);

            for (int i = 0; i < leftList.Count; i++)
                resultList.Add(leftList[i] + rightList[i]);
            NormalizeList(resultList);
            bfAns = CreateFromString(IntListToString(resultList, resultList.Count - desiredFrac));
            if (Sign < 0)
                bfAns.SwitchSign();
            return bfAns;
        }
        public override BigNumber Dif(BigNumber op)
        {
            BigFloat bfLeft = this;
            BigFloat bfRight = (BigFloat)op;
            BigFloat bfAns;
            
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
            var resultList = new List<int>(leftList.Count);

            for (int i = 0; i < leftList.Count; i++)
                resultList.Add(leftList[i] - rightList[i]);
            NormalizeList(resultList);
            bfAns = CreateFromString(IntListToString(resultList, resultList.Count - desiredFrac));
            if (sign < 0)
                bfAns.SwitchSign();
            return bfAns;
        }
        public override BigNumber Mul(BigNumber op)
        {
            return new BigFloat();
        }
        public override BigNumber Div(BigNumber op)
        {
            return new BigFloat();
        }

        /* === Operators === */
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

        /* === Variables === */
        private static readonly string validStringRegEx = @"^\s*[+-]?[0-9]+(\.[0-9]+)?\s*$";
        private static readonly string cleanStringRegEx = @"([1-9]+[0-9]*(\.[0-9]*[1-9]+)?|0\.[0-9]*[1-9]+)";

        private volatile int _dotPos = 0;
        private volatile int _fracLen = -1;

        /* === Mutexes === */
        private readonly Object dotPosMutex = new Object();
        private readonly Object fracLenMutex = new Object();

        /* === Properties === */
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
                    lock(fracLenMutex)
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
    }
}
