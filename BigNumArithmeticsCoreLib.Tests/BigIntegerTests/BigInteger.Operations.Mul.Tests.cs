using System;
using System.Text;
using NUnit.Framework;

namespace BignumArithmetics.BigIntegerTests
{
    [TestFixture]
    public class BigIntegerOperationsMulTests
    {
        static Random rnd = new Random((int)DateTime.Now.Ticks);
        static void DoTesting(string left, string right, string result)
        {
            BigInteger A = new BigInteger(left);
            BigInteger B = new BigInteger(right);

            BigInteger C = A * B;
            Assert.AreEqual(result, C.ToString());
        }
        static void RandomTest()
        {
            long a = rnd.Next(0, Int32.MaxValue);
            long b = rnd.Next(0, Int32.MaxValue);
            a -= Int32.MaxValue / 2;
            b -= Int32.MaxValue / 2;
            long c = a * b;

            DoTesting(a.ToString(), b.ToString(), c.ToString());
        }

        [Test]
        public void Zero_Zero()
        {
            DoTesting("0", "0", "0");
        }

        [Test]
        public void Zero_m5()
        {
            DoTesting("0", "-5", "0");
        }

        [Test]
        public void m5_zero()
        {
            DoTesting("-5", "0","0");
        }

        [Test]
        public void m5_p6()
        {
            DoTesting("-5", "6", "-30");
        }

        [Test]
        public void random_10000_tests()
        {
            for (int i = 0; i < 10000; i++)
                RandomTest();
        }
    }
}
