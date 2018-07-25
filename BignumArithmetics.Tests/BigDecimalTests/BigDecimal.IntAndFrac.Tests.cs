using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BignumArithmetics.BigDecimalTests
{
    [TestClass]
    public class BigDecimalIntFracTests
    {
        [TestMethod]
        public void Zero()
        {
            BigDecimal actual = new BigDecimal("0");

            Assert.AreEqual(1, actual.Integer);
            Assert.AreEqual(0, actual.Fractional);
        }
        [TestMethod]
        public void Hundred()
        {
            BigDecimal actual = new BigDecimal("-100");

            Assert.AreEqual(3, actual.Integer);
            Assert.AreEqual(0, actual.Fractional);
        }
        [TestMethod]
        public void ZeroDot156()
        {
            BigDecimal actual = new BigDecimal("-0.156");

            Assert.AreEqual(1, actual.Integer);
            Assert.AreEqual(3, actual.Fractional);
        }
        [TestMethod]
        public void BigNumber()
        {
            BigDecimal actual = new BigDecimal("923742.42382");

            Assert.AreEqual(6, actual.Integer);
            Assert.AreEqual(5, actual.Fractional);
        }       
    }
}
