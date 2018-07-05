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
        public FixedPointNumber(string Str = "0")
        {
            if (Str == null || Str == "")
                throw new IncorrectNumberFormatException("FixedPointNumber cannot be created from an empty string");
            this.RawString = Str.Trim();
            this.StringToDigits();
        }

        /* methods */

        public static char ToChar(byte Digit) { return Convert.ToChar(Digit + '0'); }
        public static byte ToDigit(char C) { return Convert.ToByte(C - '0'); }

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
        void StringToDigits()
        {
            int DotPos = RawString.IndexOf(".");

            if (DotPos < 0)
                DotPos = RawString.Length;
            Digits = new List<byte>();
            this.Dot = -1;
            //at first convert the integer part
            for (int i = Math.Max(0, DotPos - MaxSize); i < DotPos; i++)
                if (Char.IsDigit(RawString[i]))
                    Digits.Add(FixedPointNumber.ToDigit(RawString[i]));
                else
                    throw new IncorrectNumberFormatException("Number can only contain digits 0..9 and optional delimiter '.' or ','.");
            for (int i = DotPos; i < RawString.Length && Digits.Count < MaxSize; i++)
                if (Char.IsDigit(RawString[i]))
                    Digits.Add(FixedPointNumber.ToDigit(RawString[i]));
                else if (RawString[i] == '.' || (RawString[i] == ','))
                {
                    if (Digits.Count == 0)
                        throw new IncorrectNumberFormatException("Number cannot begin of dot. Use \"0.*\" format.");
                    if (this.Dot > 0)
                        throw new IncorrectNumberFormatException("Number cannot have more than one delimiter");
                    this.Dot = i;
                }
                else
                    throw new IncorrectNumberFormatException("Number can only contain digits 0..9 and optional delimiter '.' or ','.");
        }

        /* variables */

        private List<byte> Digits = new List<byte>();
        private int Dot { get; set; }
        public string RawString { get; private set; }
        public const int MaxSize = 100000;

        /* overloading */

        //thoughts for abstract class
        //if gettype(a) != gettype(b) exception
        //else cast to relevant class object and call operator

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

        /*public static FixedPointNumber operator +(FixedPointNumber A, FixedPointNumber B)
        {
            //todo: two getters : of len before dot and of len after dot
            //todo: store number reversed
            //todo: begin summing from the lowest digit
            //todo: normalize digits at the end
        }*/

    }
}
#pragma warning restore CS0660 // Тип определяет оператор == или оператор !=, но не переопределяет Object.Equals(object o)
#pragma warning restore CS0661 // Тип определяет оператор == или оператор !=, но не переопределяет Object.GetHashCode()
