using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

namespace net.NataliVol4ica.BignumArithmetics
{
    public class BigFloat : BigNumber
    {
        public BigFloat(string str = "0")
        {
            if (str is null ||
                !Regex.IsMatch(str, validStringRegEx, RegexOptions.None))
                throw new NumberFormatException("Cannot create BigFloat of \"" + str + "\"");
            CleanString = CleanNumericString(str, ref _sign);
        }
        public BigFloat(BigFloat from)
        {
            _cleanString = from.ToString();
            _sign = from.Sign;
            _dotPos = from.DotPos;
            _fracLen = from.Fracial;
        }

        /* === Static Methods === */
        //this method returns the list reversed!
        //[UNTESTED]
        public static List<int> BigFloatToIntList(BigFloat num, int DesiredInt, int DesiredFrac)
        {
            List<int> ret = new List<int>();
            int IntZeros, FracZeros;

            if (num is null)
                return ret;
            IntZeros = Math.Max(num.Integer, DesiredInt) - num.Integer;
            FracZeros = Math.Max(num.Fracial, DesiredFrac) - num.Fracial;
            ret.AddRange(Enumerable.Repeat(0, FracZeros));
            for (int i = num.CleanString.Length - 1; i >= 0; i--)
                if (num.CleanString[i] != '.')
                    ret.Add(ToDigit(num.CleanString[i]));
            ret.AddRange(Enumerable.Repeat(0, IntZeros));
            return ret;
        }
        //[UNTESTED]
        public static void NormalizeList(List<int> digits)
        {
            int i;

            if (digits is null || digits.Count == 0)
                return;
            for (i = 0; i < digits.Count - 1; i++)
            {
                digits[i + 1] += digits[i] / 10;
                digits[i] %= 10;
            }
            while (digits[i] > 9)
            {
                digits.Add(digits[i] / 10);
                digits[i] %= 10;
                i++;
            }
        }
        //[UNTESTED]
        public static string IntListToString(List<int> digits, int DotPos)
        {
            int i;
            int reverseDot;
            StringBuilder sb = new StringBuilder();

            if (digits is null || digits.Count == 0)
                return "";
            reverseDot = digits.Count - DotPos;
            for (i = digits.Count - 1; i >= reverseDot; i--)
                sb.Append(ToChar(digits[i]));
            if (i > 0)
            {
                sb.Append(".");
                while (i > 0)
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
        public override BigNumber Sum(BigNumber op)
        {
            return new BigFloat();
        }
        public override BigNumber Dif(BigNumber op)
        {
            return new BigFloat();
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

            ret.Sign = -ret.Sign;
            return ret;
        }

        /* === Variables === */
        public static readonly string validStringRegEx = @"^\s*[+-]?[0-9]+(\.[0-9]+)?\s*$";
        public static readonly string cleanStringRegEx = @"([1-9]+[0-9]*(\.[0-9]*[1-9]+)?|0\.[0-9]*[1-9]+)";

        private volatile int _dotPos = 0;
        private volatile int _fracLen = -1;

        /* === Mutexes === */
        private Object dotPosMutex = new Object();
        private Object fracLenMutex = new Object();

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
        protected override string CleanStringRegEx
        {
            get
            {
                return cleanStringRegEx;
            }
        }
        public int Integer
        {
            get
            {
                return DotPos;
            }
        }
        public int Fracial
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
        }
    }
}
