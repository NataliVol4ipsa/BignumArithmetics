using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

namespace net.NataliVol4ica.BignumArithmetics
{
    public class BigFloat : BigNumber
    {
        //todo: add constructor taking as parameter numeric types
        public BigFloat()
        {
            CleanString = "0";
        }
        public BigFloat(BigFloat from)
        {
            CleanString = from.ToString();
            if (from.Sign < 0)
                SwitchSign();
            DotPos = from.DotPos;
            Fracial = from.Fracial;
        }
        //supersafe constructor
        private BigFloat(string str, int sign)
        {
            CleanString = str;
            if (sign < 0)
                SwitchSign();
        }

        /* === Static Methods === */       
        //factory method
        public static BigFloat CreateFromString(string str)
        {
            if (str is null ||
                !Regex.IsMatch(str, validStringRegEx, RegexOptions.None))
                return null;
            str = CleanNumericString(str, out int sign);
            return new BigFloat(str, sign);
        }
        //cleans the string
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
        //this method returns the list reversed!
        //[UNTESTED]
        public static List<int> BigFloatToIntList(BigFloat num, int desiredInt, int desiredFrac)
        {
            List<int> ret = new List<int>();
            int IntZeros, FracZeros;

            if (num is null)
                return ret;
            IntZeros = Math.Max(num.Integer, desiredInt) - num.Integer;
            FracZeros = Math.Max(num.Fracial, desiredFrac) - num.Fracial;
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
        public static string IntListToString(List<int> digits, int dotPos)
        {
            int i;
            int reverseDot;
            StringBuilder sb = new StringBuilder();

            if (digits is null || digits.Count == 0)
                return "";
            reverseDot = digits.Count - dotPos;
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

            ret.SwitchSign();
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
        /*protected override string CleanStringRegEx
        {
            get
            {
                return cleanStringRegEx;
            }
        }
       */
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
            private set
            {
                _fracLen = value;
            }
        }
    }
}
