using System;
using System.Collections.Generic;

namespace net.NataliVol4ica.BignumArithmetics
{
    class FixedPointNumber
    {
        public FixedPointNumber (string str = "0")
        {
            this.RawString = str;
            this.StringToDigits();
        }

        /* methods */

        public static char ToChar(byte digit) { return Convert.ToChar(digit + '0'); }
        public static byte ToDigit(char c) { return Convert.ToByte(c - '0'); }

        void StringToDigits()
        {
            digits = new List<byte>();
            this.Dot = -1;
            for (int i = 0; i < RawString.Length; i++)
                if (Char.IsWhiteSpace(RawString[i]))
                    continue;
                else if (Char.IsDigit(RawString[i]))
                    digits.Add(FixedPointNumber.ToDigit(RawString[i]));
                else if (RawString[i] == '.')
                {
                    //todo: if this.dot > 0 -> exception else
                    this.Dot = i;
                }
            //todo: isalpha, and other -> exception
            //todo: set max len and precision
        }

        /* variables */

        private List<byte> digits = new List<byte>();
        private int Dot { get; set; }
        public string RawString { get; private set; }

        /* overloading */
        
        //thoughts for abstract class
        //if gettype(a) != gettype(b) exception
        //else cast to relevant class object and call operator

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

        public byte this[byte index]
        {
            get { return digits[index]; }
            set { digits[index] = value; }
        }
    }
}
