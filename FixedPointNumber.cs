using System;
using System.Collections.Generic;

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

        /* === Methods === */
        public static char ToChar(byte Digit) { return Convert.ToChar(Digit + '0'); }
        public static byte ToDigit(char C) { return Convert.ToByte(C - '0'); }
        public static void Swap(ref FixedPointNumber A, ref FixedPointNumber B)
        {
            FixedPointNumber buf = A;
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

        //todo: parse sign!!!
        //todo: func to cut heading and closing zeros
        //todo: func to convert digits to string
        void StringToDigits()
        {
            int DotPos = RawString.IndexOf(".");

            Digits = new List<byte>();
            this.Dot = -1;
            if (DotPos < 0)
                DotPos = RawString.Length;
            //Convertin the integer part
            for (int i = Math.Max(0, DotPos - MaxSize); i < DotPos; i++)
                if (Char.IsDigit(RawString[i]))
                    Digits.Add(FixedPointNumber.ToDigit(RawString[i]));
                else
                    throw new IncorrectNumberFormatException("Number can only contain digits 0..9 and optional delimiter '.' or ','.");
            //Converting the frac part
            for (int i = DotPos; i < RawString.Length && Digits.Count < MaxSize; i++)
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
        private List<byte> Digits = new List<byte>();
        private int Dot { get; set; }
        public string RawString { get; private set; }
        public const int MaxSize = 100000;

        /* === Overloading tools === */

        public void SetAsSum(FixedPointNumber A, FixedPointNumber B)
        {
            //todo: store number reversed
            //todo: begin summing from the lowest digit
            //todo: normalize digits at the end

            int maxFrac = Math.Max(A.GetFracLen(), B.GetFracLen());


        }

        /* === Overloading === */
        public byte this[byte index]
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
        //todo: overload > and <
        public static FixedPointNumber operator +(FixedPointNumber A, FixedPointNumber B)
        {
            FixedPointNumber ans = new FixedPointNumber();

            if (A.GetFracLen() < B.GetFracLen())
                FixedPointNumber.Swap(ref A, ref B);
            ans.SetAsSum(A, B);
            return (ans);
        }
    }
}
#pragma warning restore CS0660 // Тип определяет оператор == или оператор !=, но не переопределяет Object.Equals(object o)
#pragma warning restore CS0661 // Тип определяет оператор == или оператор !=, но не переопределяет Object.GetHashCode()
