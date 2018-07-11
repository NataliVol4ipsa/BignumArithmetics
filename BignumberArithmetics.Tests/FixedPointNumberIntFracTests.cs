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
            FixedPointNumber actual = new FixedPointNumber("0");

            Assert.AreEqual(1, actual.Integer);
            Assert.AreEqual(0, actual.Fracial);
        }
        [TestMethod]
        public void Hundred()
        {
            FixedPointNumber actual = new FixedPointNumber("-100");

            Assert.AreEqual(3, actual.Integer);
            Assert.AreEqual(0, actual.Fracial);
        }
        [TestMethod]
        public void ZeroDot156()
        {
            FixedPointNumber actual = new FixedPointNumber("-0.156");

            Assert.AreEqual(1, actual.Integer);
            Assert.AreEqual(3, actual.Fracial);
        }
        [TestMethod]
        public void BigNumber()
        {
            FixedPointNumber actual = new FixedPointNumber("923742.42382");

            Assert.AreEqual(6, actual.Integer);
            Assert.AreEqual(5, actual.Fracial);
        }       
    }
}
