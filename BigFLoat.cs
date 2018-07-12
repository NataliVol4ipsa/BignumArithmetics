using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

#pragma warning disable CS0660 // Тип определяет оператор == или оператор !=, но не переопределяет Object.Equals(object o)
#pragma warning disable CS0661 // Тип определяет оператор == или оператор !=, но не переопределяет Object.GetHashCode()

//todo: move exceptions elsewhere

//using System.Linq ; var lastItem = integerList.Last();

namespace net.NataliVol4ica.BignumArithmetics
{
    public class NumberFormatException : Exception
    {
        public NumberFormatException() { }
        public NumberFormatException(string message)
            : base(message) { }
        public NumberFormatException(string message, Exception inner)
            : base(message, inner) { }
    }

    public class BigFloat : BigNumber
    {
        //todo: dot is better as -1 or at the end??
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

//namespace net.NataliVol4ica.BignumArithmetics
//{
// 
//        public FixedPointNumber Copy()
//        {
//            List<int> newList = new List<int>(this.Digits);
//            newList.Reverse();
//            return new FixedPointNumber(newList, this.Dot);
//        }
// 
//        private void Normalize()
//        {
//            int maxFrac = this.Dot;

//            while (this.Digits[this.Digits.Count - 1] == 0 && maxFrac > 0)
//            {
//                maxFrac--;
//                this.Digits.RemoveAt(Digits.Count - 1);
//            }
//            this.Digits.Reverse();
//            while (this.Digits[this.Digits.Count - 1] == 0
//                && this.Digits.Count > maxFrac
//                && this.Digits.Count > 1)
//                this.Digits.RemoveAt(Digits.Count - 1);
//            this.Digits.Reverse();
//            if (this.Digits.Count == 1 && this.Digits[0] == 0)
//                this.Sign = 1;
//        }

//        /* === Operations === */
//        public override BigNumber Sum(BigNumber op)
//        {
//            FixedPointNumber A = this;
//            FixedPointNumber B = (FixedPointNumber)op;
//            FixedPointNumber C;

//            if (A.Sign > 0 && B.Sign > 0)
//                C = BigSum.Count(A, B);
//            else if (A.Sign < 0 && B.Sign < 0)
//                C = -BigSum.Count(A, B);
//            else if (A.Sign > 0 && B.Sign < 0)
//                C = (A - -B);
//            else
//                C = (B - -A);
//            return C;
//        }
//        public override BigNumber Dif(BigNumber op)
//        {
//            FixedPointNumber A = this;
//            FixedPointNumber B = (FixedPointNumber)op;
//            FixedPointNumber C;

//            if (A.Sign > 0 && B.Sign > 0)
//                C = A > B ?
//                    BigDif.Count(A, B)
//                  : -BigDif.Count(B, A);
//            else if (A.Sign < 0 && B.Sign < 0)
//                C = A > B ?
//                    -BigDif.Count(B, A)
//                  : BigDif.Count(A, B);
//            else if (A.Sign > 0 && B.Sign < 0)
//                C = BigSum.Count(A, B);
//            else
//                C = -BigSum.Count(B, A);
//            return C;
//        }
// 
//        
//        //binary
//        public static bool operator ==(FixedPointNumber cmpA, FixedPointNumber cmpB)
//        {
//            if (string.Compare(cmpA.RawString, cmpB.RawString) == 0)
//                return true;
//            return false;
//        }
//        public static bool operator !=(FixedPointNumber cmpA, FixedPointNumber cmpB)
//        {
//            if (string.Compare(cmpA.RawString, cmpB.RawString) == 0)
//                return false;
//            return true;
//        }
//        public static bool operator >(FixedPointNumber A, FixedPointNumber B)
//        {
//            // A > 0
//            if (A.Sign > 0)
//            {
//                if (B.Sign < 0)
//                    return true;
//                if (A.GetIntLen() > B.GetIntLen())
//                    return true;
//                if (A.GetIntLen() < B.GetIntLen())
//                    return false;
//                if (String.Compare(A.RawString, B.RawString) > 0)
//                    return true;
//                return false;
//            }
//            // A < 0
//            if (B.Sign > 0)
//                return false;
//            if (A.GetIntLen() > B.GetIntLen())
//                return false;
//            if (A.GetIntLen() < B.GetIntLen())
//                return true;
//            if (String.Compare(A.RawString, B.RawString) > 0)
//                return false;
//            return true;
//        }
//        public static bool operator <(FixedPointNumber A, FixedPointNumber B)
//        {
//            // A > 0
//            if (A.Sign > 0)
//            {
//                if (B.Sign < 0)
//                    return false;
//                if (A.GetIntLen() > B.GetIntLen())
//                    return false;
//                if (A.GetIntLen() < B.GetIntLen())
//                    return true;
//                if (String.Compare(A.RawString, B.RawString) > 0)
//                    return false;
//                return true;
//            }
//            // A < 0
//            if (B.Sign > 0)
//                return true;
//            if (A.GetIntLen() > B.GetIntLen())
//                return true;
//            if (A.GetIntLen() < B.GetIntLen())
//                return false;
//            if (String.Compare(A.RawString, B.RawString) > 0)
//                return true;
//            return false;
//        }
//        public static FixedPointNumber operator +(FixedPointNumber A, FixedPointNumber B)
//        {
//            if (A == null || B == null)
//                return null;
//            FixedPointNumber result = (FixedPointNumber)A.Sum(B);
//            return result;
//        }
//        public static FixedPointNumber operator -(FixedPointNumber A, FixedPointNumber B)
//        {
//            if (A == null || B == null)
//                return null;
//            FixedPointNumber result = (FixedPointNumber)A.Dif(B);
//            return result;
//        }

//        /* === Vaiables === */
//        private int Dot;
//    }
//}
#pragma warning restore CS0660 // Тип определяет оператор == или оператор !=, но не переопределяет Object.Equals(object o)
#pragma warning restore CS0661 // Тип определяет оператор == или оператор !=, но не переопределяет Object.GetHashCode()
