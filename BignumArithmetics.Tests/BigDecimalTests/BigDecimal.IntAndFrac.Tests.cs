using System;
using NUnit.Framework;

namespace BignumArithmetics.BigDecimalTests
{
    [TestFixture]
    public class BigDecimalIntFracTests
    {
        [Test]
        public void Zero()
        {
            BigDecimal actual = new BigDecimal("0");

            Assert.AreEqual(1, actual.Integer);
            Assert.AreEqual(0, actual.Fractional);
        }
        [Test]
        public void Hundred()
        {
            BigDecimal actual = new BigDecimal("-100");

            Assert.AreEqual(3, actual.Integer);
            Assert.AreEqual(0, actual.Fractional);
        }
        [Test]
        public void ZeroDot156()
        {
            BigDecimal actual = new BigDecimal("-0.156");

            Assert.AreEqual(1, actual.Integer);
            Assert.AreEqual(3, actual.Fractional);
        }
        [Test]
        public void BigNumber()
        {
            BigDecimal actual = new BigDecimal("923742.42382");

            Assert.AreEqual(6, actual.Integer);
            Assert.AreEqual(5, actual.Fractional);
        }       
    }
}
