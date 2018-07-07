using System;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

#pragma warning disable CS0660 // Тип определяет оператор == или оператор !=, но не переопределяет Object.Equals(object o)
#pragma warning disable CS0661 // Тип определяет оператор == или оператор !=, но не переопределяет Object.GetHashCode()

namespace net.NataliVol4ica.BignumArithmetics
{
    public class IncorrectNumberFormatException : Exception
    {
        public IncorrectNumberFormatException() { }
        public IncorrectNumberFormatException(string message)
            : base(message) { }
        public IncorrectNumberFormatException(string message, Exception inner)
            : base(message, inner) { }
    }

    class FixedPointNumber
    {
        /* === Constructors === */
        public FixedPointNumber(string Str = "0")
        {
            if (Str == null || Str == "")
                throw new IncorrectNumberFormatException("FixedPointNumber cannot be created from an empty string");
            this.RawString = Str.Trim();
            this.StringToDigits();
        }
        public FixedPointNumber(List<int> digits, int Dot = -1)
        {
            //todo: add input validation
            StringBuilder sb = new StringBuilder();
            this.Dot = Dot;
            Dot = Dot < 0 ? digits.Count : Dot;
            for (int i = 0; i < Dot; i++)            
                sb.Append(FixedPointNumber.ToChar(digits[i]));
            if (this.Dot > 0 && this.Dot < digits.Count)
                sb.Append(".");
            for (int i = Dot; i < digits.Count; i++)
                sb.Append(FixedPointNumber.ToChar(digits[i]));
            this.Digits = digits;
            this.Digits.Reverse();
            this.RawString = sb.ToString();
        }

        /* === Methods === */
        public static char ToChar(int Digit) { return Convert.ToChar(Digit + '0'); }
        public static byte ToDigit(char C) { return Convert.ToByte(C - '0'); }
        public static void Swap<T>(ref T A, ref T B)
        {
            T buf = A;
            A = B;
            B = buf;
        }
        public int GetIntLen()
        {
            if (this.Dot < 0)
                return this.Digits.Count;
            return this.Dot;
        }
        public int GetFracLen()
        {
            if (this.Dot < 0)
                return 0;
            return this.Digits.Count - this.Dot;
        }
        public FixedPointNumber Copy()
        {
            return new FixedPointNumber(this.RawString);
        }

        //Regex.Replace(RawString, @"\s+", ""); remove ws-s
        //todo: func to cut heading and closing zeros
        //todo: func to convert digits to string

        void StringToDigits()
        {
            int DotPos;
            int i = 0;

            Digits = new List<int>();
            this.Sign = 1;
            while (i < RawString.Length && (RawString[i] == '+' || RawString[i] == '-'))
            {
                if (RawString[i] == '-')
                    this.Sign = -this.Sign;
                i++;
            }
            RawString.Remove(0, i);
            if (this.Sign < 0)
                RawString = "-" + RawString;
            //todo: if (i == RawString.Length) exception
            DotPos = RawString.IndexOf(".");
            this.Dot = -1;
            if (DotPos < 0)
                DotPos = RawString.Length;
            //Converting the integer part
            for (; i < DotPos; i++)
                if (Char.IsDigit(RawString[i]))
                    Digits.Add(FixedPointNumber.ToDigit(RawString[i]));
                else
                    throw new IncorrectNumberFormatException("Number can only contain digits 0..9 and optional delimiter '.' or ','.");
            //Converting the frac part
            for (; i < RawString.Length; i++)
                if (Char.IsDigit(RawString[i]))
                    Digits.Add(FixedPointNumber.ToDigit(RawString[i]));
                else if (RawString[i] == '.' || (RawString[i] == ','))
                {
                    if (Digits.Count == 0)
                        throw new IncorrectNumberFormatException("Number cannot begin with dot. Use \"0.*\" format.");
                    if (this.Dot > 0)
                        throw new IncorrectNumberFormatException("Number cannot have more than one delimiter");
                    this.Dot = i;
                }
                else
                    throw new IncorrectNumberFormatException("Number can only contain digits 0..9 and optional delimiter '.' or ','.");
            //todo: if dot and no digits - set dot as null
            Digits.Reverse();
        }

        /* === Variables === */
        private List<int> Digits = new List<int>();
        private int Dot { get; set; }
        public int Sign { get; private set; }
        public string RawString { get; private set; }
        //todo: change to max precision? meeeh

        /* === Overloading tools === */
        
        private static void NormalizeReverse(List<int> digits, ref int maxFrac)
        {
            while (digits[digits.Count - 1] == '0' && digits.Count > maxFrac)
                digits.RemoveAt(digits.Count - 1);
            digits.Reverse();
            while (digits[digits.Count - 1] == '0' && maxFrac > 0)
            {
                maxFrac--;
                digits.RemoveAt(digits.Count - 1);
            }
        }

        static FixedPointNumber Sum(FixedPointNumber A, FixedPointNumber B)
        {
            //todo: parse max new len ?
            List<int> sum = new List<int>();
            int i = 0;
            int j = 0;

            int maxFrac;
            int indexDif;
            int plus;

            if (A.GetFracLen() < B.GetFracLen())
                FixedPointNumber.Swap(ref A, ref B);
            maxFrac = A.GetFracLen();
            indexDif = A.GetFracLen() - B.GetFracLen();
            plus = 0;

            //todo: remember to check if A[i] -> A.ToString[i] is fine
            while (i < indexDif)
            {
                sum.Add(A[i]);
                i++;
            }
            while (i < maxFrac)
            {
                sum.Add(A[i] + B[j] + plus);
                plus = sum[i] / 10;
                sum[i] %= 10;
                i++;
                j++;
            }
            /* eo dot */
            if (A.GetIntLen() < B.GetIntLen())
            {
                FixedPointNumber.Swap(ref A, ref B);
                FixedPointNumber.Swap(ref i, ref j);
            }
            while (j<B.Digits.Count)
            {
                sum.Add(A[i] + B[j] + plus);
                plus = sum[sum.Count - 1] / 10;
                sum[sum.Count - 1] %= 10;
                i++;
                j++;
            }
            while (i<A.Digits.Count)
            {
                sum.Add(A[i] + plus);
                plus = sum[sum.Count - 1] / 10;
                sum[sum.Count - 1] %= 10;
                i++;
            }
            while (plus > 0)
            {
                sum.Add(plus % 10);
                plus /= 10;
            }
            FixedPointNumber.NormalizeReverse(sum, ref maxFrac);
            return (new FixedPointNumber(sum, sum.Count - maxFrac));
        }
        static FixedPointNumber Dif(FixedPointNumber A, FixedPointNumber B)
        {
            return new FixedPointNumber();
        }

        /* === Overloading === */
        public int this[int index]
        {
            get { return Digits[index]; }
            set { Digits[index] = value; }
        }
        public override string ToString() { return RawString; }
        public static bool operator ==(FixedPointNumber cmpA, FixedPointNumber cmpB)
        {
            if (string.Compare(cmpA.RawString, cmpB.RawString) == 0)
                return true;
            return false;
        }
        public static bool operator !=(FixedPointNumber cmpA, FixedPointNumber cmpB)
        {
            if (string.Compare(cmpA.RawString, cmpB.RawString) == 0)
                return false;
            return true;
        }
        public static FixedPointNumber operator -(FixedPointNumber num)
        {
            return (new FixedPointNumber("-" + num.RawString));
        }

        //todo: overload > and < ?
        //todo: overload unar -
        public static FixedPointNumber operator +(FixedPointNumber A, FixedPointNumber B)
        {
            if (A.Sign > 0 && B.Sign > 0)
                return Sum(A, B);
            if (A.Sign < 0 && B.Sign < 0)
                return -Sum(A, B);
            if (A.Sign > 0 && B.Sign < 0)
                return Dif(A, -B);
            return Dif(B, -A);
        }
        
        public static FixedPointNumber operator -(FixedPointNumber A, FixedPointNumber B)
        {
            if (A.Sign > 0 && B.Sign > 0)
                return Dif(A, B);
            if (A.Sign < 0 && B.Sign < 0)
                return -Sum(A, B);
            if (A.Sign > 0 && B.Sign < 0)
                return Dif(A, -B);
            return Dif(B, -A);
        }
    }
}
#pragma warning restore CS0660 // Тип определяет оператор == или оператор !=, но не переопределяет Object.Equals(object o)
#pragma warning restore CS0661 // Тип определяет оператор == или оператор !=, но не переопределяет Object.GetHashCode()
