using System;
using System.Collections.Generic;

namespace net.NataliVol4ica.BignumArithmetics
{
    /// <summary>
    /// Abstract class for big numbers
    /// </summary>
    
    public abstract class BigNumber
    {
        /* === Methods === */
        public static char ToChar(int Digit)
        {
            return Convert.ToChar(Digit + '0');
        }
        public static byte ToDigit(char C)
        {
            return Convert.ToByte(C - '0');
        }
        public static void Swap<T>(ref T A, ref T B)
        {
            T buf = A;
            A = B;
            B = buf;
        }
        public int this[int index]
        {
            get { return Digits[index]; }
            set { Digits[index] = value; }
        }

        /* === Variables === */
        protected List<int> Digits = new List<int>();
        protected string RawString = null;
        public int Sign { get; protected set; }
        public int Size
        {
            get
            {
                return Digits.Count;
            }
        }
    }
}
