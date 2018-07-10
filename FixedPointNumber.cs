using System;
using System.Text;
using System.Collections.Generic;

#pragma warning disable CS0660 // Тип определяет оператор == или оператор !=, но не переопределяет Object.Equals(object o)
#pragma warning disable CS0661 // Тип определяет оператор == или оператор !=, но не переопределяет Object.GetHashCode()

//using System.Linq ; var lastItem = integerList.Last();

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

    public class FixedPointNumber : BigNumber
    {
        /* === Constructors === */
        //todo: add input validation
        //todo: dot is better as -1 or at the end??
        /// <exception cref="BigNumberException.IncorrectNumberFormatException" />
        public FixedPointNumber(string str = "0")
        {
            //add zeros cutting
            if (str == null || str == "")
                throw new IncorrectNumberFormatException("FixedPointNumber cannot be created from an empty string");
            this.StringToDigits(str.Trim());
            this.Normalize();
        }
        public FixedPointNumber(List<int> digits, int Dot = -1)
        {
            this.Dot = Dot;
            this.Digits = digits;
            this.Digits.Reverse();
            this.Normalize();
        }

        /* === Methods === */
        //Abs seems unneeded
        public static FixedPointNumber Abs(FixedPointNumber num)
        {
            if (num.Sign > 0)
                return num;
            return -num;
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
            List<int> newList = new List<int>(this.Digits);
            newList.Reverse();
            return new FixedPointNumber(newList, this.Dot);
        }
        private void StringToDigits(string str)
        {
            int DotPos;
            int i = 0;

            Digits = new List<int>();
            this.Sign = 1;
            while (i < str.Length && (str[i] == '+' || str[i] == '-'))
            {
                if (str[i] == '-')
                    this.Sign = -this.Sign;
                i++;
            }
            str = str.Remove(0, i);
            i = 0;
           //todo: if (i == str.Length) exception
            DotPos = str.IndexOf(".");
            this.Dot = -1;
            if (DotPos < 0)
                DotPos = str.Length;
            //Converting the integer part
            for (; i < DotPos; i++)
                if (Char.IsDigit(str[i]))
                    Digits.Add((int)Char.GetNumericValue(str[i]));
                else
                    throw new IncorrectNumberFormatException("Number can only contain digits 0..9 and optional delimiter '.' or ','.");
            //Converting the frac part
            for (; i < str.Length; i++)
                if (Char.IsDigit(str[i]))
                    Digits.Add((int)Char.GetNumericValue(str[i]));
                else if (str[i] == '.' || str[i] == ',')
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
        private void Normalize()
        {
            int maxFrac = this.Dot;

            while (this.Digits[this.Digits.Count - 1] == 0 && maxFrac > 0)
            {
                maxFrac--;
                this.Digits.RemoveAt(Digits.Count - 1);
            }
            this.Digits.Reverse();
            while (this.Digits[this.Digits.Count - 1] == 0
                && this.Digits.Count > maxFrac
                && this.Digits.Count > 1)
                this.Digits.RemoveAt(Digits.Count - 1);
            this.Digits.Reverse();
            if (this.Digits.Count == 1 && this.Digits[0] == 0)
                this.Sign = 1;
        }

        /* === Operations === */
        public override BigNumber Sum(BigNumber op)
        {
            FixedPointNumber A = this;
            FixedPointNumber B = (FixedPointNumber)op;
            FixedPointNumber C;

            if (A.Sign > 0 && B.Sign > 0)
                C = BigSum.Count(A, B);
            else if (A.Sign < 0 && B.Sign < 0)
                C = -BigSum.Count(A, B);
            else if (A.Sign > 0 && B.Sign < 0)
                C = (A - -B);
            else
                C = (B - -A);
            return C;
        }
        public override BigNumber Dif(BigNumber op)
        {
            FixedPointNumber A = this;
            FixedPointNumber B = (FixedPointNumber)op;
            FixedPointNumber C;

            if (A.Sign > 0 && B.Sign > 0)
                C = A > B ?
                    BigDif.Count(A, B)
                  : -BigDif.Count(B, A);
            else if (A.Sign < 0 && B.Sign < 0)
                C = A > B ?
                    -BigDif.Count(B, A)
                  : BigDif.Count(A, B);
            else if (A.Sign > 0 && B.Sign < 0)
                C = BigSum.Count(A, B);
            else
                C = -BigSum.Count(B, A);
            return C;
        }
        public override BigNumber Mul(BigNumber op)
        {
            FixedPointNumber num = (FixedPointNumber)op;
            return BigMul.Count(this, num);
        }
        public override BigNumber Div(BigNumber op)
        {
            FixedPointNumber num = (FixedPointNumber)op;
            return BigDiv.Count(this, num);
        }

        /* === Overloading === */
        public override string ToString()
        {
            StringBuilder sb;
            int Dot;

            if (this.RawString != null)
                return this.RawString;
            sb = new StringBuilder();
            Dot = this.Dot < 0 ? 0 : Digits.Count - this.Dot;
            for (int i = Digits.Count - 1; i >= Dot; i--)
                sb.Append(FixedPointNumber.ToChar(Digits[i]));
            if (this.Dot > 0 && this.Dot < Digits.Count)
                sb.Append(".");
            for (int i = Dot - 1; i >= 0; i--)
                sb.Append(FixedPointNumber.ToChar(Digits[i]));
            this.RawString = sb.ToString();
            return this.RawString;
        }
        //unar
        public static FixedPointNumber operator -(FixedPointNumber num)
        {
            return (new FixedPointNumber("-" + num.ToString()));
        }
        //binary
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
        public static bool operator >(FixedPointNumber A, FixedPointNumber B)
        {
            // A > 0
            if (A.Sign > 0)
            {
                if (B.Sign < 0)
                    return true;
                if (A.GetIntLen() > B.GetIntLen())
                    return true;
                if (A.GetIntLen() < B.GetIntLen())
                    return false;
                if (String.Compare(A.RawString, B.RawString) > 0)
                    return true;
                return false;
            }
            // A < 0
            if (B.Sign > 0)
                return false;
            if (A.GetIntLen() > B.GetIntLen())
                return false;
            if (A.GetIntLen() < B.GetIntLen())
                return true;
            if (String.Compare(A.RawString, B.RawString) > 0)
                return false;
            return true;
        }
        public static bool operator <(FixedPointNumber A, FixedPointNumber B)
        {
            // A > 0
            if (A.Sign > 0)
            {
                if (B.Sign < 0)
                    return false;
                if (A.GetIntLen() > B.GetIntLen())
                    return false;
                if (A.GetIntLen() < B.GetIntLen())
                    return true;
                if (String.Compare(A.RawString, B.RawString) > 0)
                    return false;
                return true;
            }
            // A < 0
            if (B.Sign > 0)
                return true;
            if (A.GetIntLen() > B.GetIntLen())
                return true;
            if (A.GetIntLen() < B.GetIntLen())
                return false;
            if (String.Compare(A.RawString, B.RawString) > 0)
                return true;
            return false;
        }
        public static FixedPointNumber operator +(FixedPointNumber A, FixedPointNumber B)
        {
            if (A == null || B == null)
                return null;
            FixedPointNumber result = (FixedPointNumber)A.Sum(B);
            return result;
        }
        public static FixedPointNumber operator -(FixedPointNumber A, FixedPointNumber B)
        {
            if (A == null || B == null)
                return null;
            FixedPointNumber result = (FixedPointNumber)A.Dif(B);
            return result;
        }

        /* === Vaiables === */
        private int Dot;
    }
}
#pragma warning restore CS0660 // Тип определяет оператор == или оператор !=, но не переопределяет Object.Equals(object o)
#pragma warning restore CS0661 // Тип определяет оператор == или оператор !=, но не переопределяет Object.GetHashCode()
