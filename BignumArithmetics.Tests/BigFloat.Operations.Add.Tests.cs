using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BignumArithmetics.Tests
{
    [TestClass]
    public class BigFLoatOperationsAddTests
    {
        static Random rnd = new Random((int)DateTime.Now.Ticks);
        static void DoTesting(string left, string right, string result)
        {
            BigFloat A = BigFloat.CreateFromString(left);
            BigFloat B = BigFloat.CreateFromString(right);

            BigFloat C = A + B;
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
            decimal C = A + B;

            DoTesting(DecimalToString(A),
                    DecimalToString(B),
                    DecimalToString(C));
        }

        [TestMethod]
        public void Zero_Zero()
        {
            DoTesting("0", "0", "0");
        }
    }
}
