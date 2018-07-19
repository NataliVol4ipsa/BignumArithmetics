using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BignumArithmetics.BigIntegerTests
{
    [TestClass]
    public class BigIntegerOperationsDivTests
    {
        static Random rnd = new Random((int)DateTime.Now.Ticks);
        static void DoTesting(string left, string right, string result)
        {
            BigInteger A = BigInteger.CreateFromString(left);
            BigInteger B = BigInteger.CreateFromString(right);

            BigInteger C = A / B;
            Assert.AreEqual(result, C.ToString());
        }
        static void RandomTest()
        {
            long a = rnd.Next(0, Int32.MaxValue);
            long b = rnd.Next(0, Int32.MaxValue);
            a -= Int32.MaxValue / 2;
            b -= Int32.MaxValue / 2;
            long c = a / b;

            DoTesting(a.ToString(), b.ToString(), c.ToString());
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
            DoTesting("-6", "5", "-1");
        }

        [TestMethod]
        public void random_10000_tests()
        {
            for (int i = 0; i < 10000; i++)
                RandomTest();
        }
    }
}
