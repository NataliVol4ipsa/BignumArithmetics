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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        public abstract BigNumber Sum(BigNumber op);
        public abstract BigNumber Dif(BigNumber op);
        public abstract BigNumber Mul(BigNumber op);
        public abstract BigNumber Div(BigNumber op);
        public static char ToChar(int Digit)
        {
            return Digit.ToString()[0];
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
        //todo: index exception
        public int this[int index]
        {
            get
            {
                if (index < 0 || index >= Digits.Count)
                    return 0;
                return Digits[index];
            }
            private set
            {
                Digits[index] = value;
            }
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

        /* === Operators === */ //????
        public static BigNumber operator +(BigNumber A, BigNumber B)
        {
            return A.Sum(B);
        }
        public static BigNumber operator -(BigNumber A, BigNumber B)
        {
            return A.Dif(B);
        }
        public static BigNumber operator *(BigNumber A, BigNumber B)
        {
            return A.Mul(B);
        }
        public static BigNumber operator /(BigNumber A, BigNumber B)
        {
            return A.Div(B);
        }
    }
}
