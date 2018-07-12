using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace net.NataliVol4ica.BignumArithmetics.Tests
{
    [TestClass]
    public class FixedPointNumberIntFracTests
    {
        [TestMethod]
        public void Zero()
        {
            BigFloat actual = BigFloat.CreateFromString("0");

            Assert.AreEqual(1, actual.Integer);
            Assert.AreEqual(0, actual.Fracial);
        }
        [TestMethod]
        public void Hundred()
        {
            BigFloat actual = BigFloat.CreateFromString("-100");

            Assert.AreEqual(3, actual.Integer);
            Assert.AreEqual(0, actual.Fracial);
        }
        [TestMethod]
        public void ZeroDot156()
        {
            BigFloat actual = BigFloat.CreateFromString("-0.156");

            Assert.AreEqual(1, actual.Integer);
            Assert.AreEqual(3, actual.Fracial);
        }
        [TestMethod]
        public void BigNumber()
        {
            BigFloat actual = BigFloat.CreateFromString("923742.42382");

            Assert.AreEqual(6, actual.Integer);
            Assert.AreEqual(5, actual.Fracial);
        }       
    }
}
