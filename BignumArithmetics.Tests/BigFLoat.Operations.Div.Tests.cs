using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

//TODO: LOG FAILS TO FILE

namespace BignumArithmetics.Tests
{
    [TestClass]
    public class BigFLoatOperationsDivTests
    {
        static Random rnd = new Random((int)DateTime.Now.Ticks);
        static void DoTesting(string left, string right, string result)
        {
            BigFloat A = BigFloat.CreateFromString(left);
            BigFloat B = BigFloat.CreateFromString(right);

            BigFloat C = A / B;
            Assert.AreEqual(C.ToString(), result);
        }
        public static string DecimalToString(decimal number)
        {
            string str = number.ToString();
            if (!str.Contains(","))
                return str;
            str = str.Replace(",", ".");
            StringBuilder sb = new StringBuilder(str);
            while (sb[sb.Length - 1] == '0')
                sb.Remove(sb.Length - 1, 1);
            if (sb[sb.Length - 1] == '.')
                sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
        static void RandomTest()
        {
            int a = rnd.Next(0, Int32.MaxValue);
            int b = rnd.Next(0, Int32.MaxValue);
            a -= Int32.MaxValue / 2;
            b -= Int32.MaxValue / 2;
            decimal A = a;
            decimal B = b;           
            decimal C = A / B;

            DoTesting(DecimalToString(C),
                    DecimalToString(B),
                    DecimalToString(A));
        }

        [TestMethod]
        [ExpectedException(typeof(DivideByZeroException))]
        public void Zero_Zero()
        {
            DoTesting("0", "0", "0");
        }

        [TestMethod]
        public void Zero_m5()
        {
            DoTesting("0", "-5", "0");
        }

        [TestMethod]
        [ExpectedException(typeof(DivideByZeroException))]
        public void m5_zero()
        {
            DoTesting("-5", "0", "0");
        }

        [TestMethod]
        public void m6_p5()
        {
            DoTesting("-6", "5", "-1.2");
        }

        [TestMethod]
        public void p20D1_p0D05()
        {
            DoTesting("20.1", "0.05", "402");
        }
        
        [TestMethod]
        public void p20D1_p5()
        {
            DoTesting("20.1", "5", "4.02");
        }

        [TestMethod]
        public void random_10000_tests()
        {
            for (int i = 0; i < 10000; i++)
                RandomTest();
        }
    }
}
