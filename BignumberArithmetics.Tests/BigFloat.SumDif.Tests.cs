using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BignumArithmetics.Tests
{
    [TestClass]
    public class BigFLoatSumDifTests
    {
        public void DoTesting(string left, string right, string sum, string dif)
        {
            BigFloat A = BigFloat.CreateFromString(left);
            BigFloat B = BigFloat.CreateFromString(right);

            BigFloat C = A + B;
            BigFloat D = A - B;
            Assert.AreEqual(sum, C.ToString());
            Assert.AreEqual(dif, D.ToString());
        }

        [TestMethod]
        public void Zero_Zero()
        {
            DoTesting("0", "0", "0", "0");
        }
        [TestMethod]
        public void Zero_m5()
        {
            DoTesting("0", "-5", "-5", "5");
        }
        [TestMethod]
        public void m5_zero()
        {
            DoTesting("-5", "0", "-5", "-5");
        }
        [TestMethod]
        public void m5_p6()
        {
            DoTesting("-5", "6", "1", "-11");
        }
        [TestMethod]
        public void p123D45_p45D678()
        {
            DoTesting("123.45", "45.678", "169.128", "77.772");
        }
        [TestMethod]
        public void m3D1_p10D005()
        {
            DoTesting("-3.1", "10.005", "6.905", "-13.105");
        }
        [TestMethod]
        public void m10D005_m3D1()
        {
            DoTesting("-10.005", "-3.1", "-13.105", "-6.905");
        }
    }
}
