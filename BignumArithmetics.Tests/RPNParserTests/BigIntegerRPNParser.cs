using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BignumArithmetics.Parsers;

namespace BignumberArithmetics.Tests.RPNParserTests
{
    [TestClass]
    public class BigIntegerRpnTests
    {
        [TestMethod]
        public void EmptyString()
        {
            var p = new BigIntegerRPNParser().Parse("");
            Assert.AreEqual("0", p.ToString());
        }
        [TestMethod]
        public void M0()
        {
            var p = new BigIntegerRPNParser().Parse("-0");
            Assert.AreEqual("0", p.ToString());
        }
        [TestMethod]
        public void M5()
        {
            var p = new BigIntegerRPNParser().Parse("-5");
            Assert.AreEqual("-5", p.ToString());
        }
        [TestMethod]
        public void P2_p_P2()
        {
            var p = new BigIntegerRPNParser().Parse("2 + 2");
            Assert.AreEqual("4", p.ToString());
        }
        [TestMethod]
        public void M2_p_M2()
        {
            var p = new BigIntegerRPNParser().Parse("-2 + -2");
            Assert.AreEqual("-4", p.ToString());
        }
        [TestMethod]
        public void M2_m_M2()
        {
            var p = new BigIntegerRPNParser().Parse("-2 * -2");
            Assert.AreEqual("4", p.ToString());
        }
        [TestMethod]
        public void BracketsTests()
        {
            var p = new BigIntegerRPNParser().Parse("(2)");
            Assert.AreEqual("2", p.ToString());
            p = new BigIntegerRPNParser().Parse("((((2))))");
            Assert.AreEqual("2", p.ToString());
            p = new BigIntegerRPNParser().Parse("((-((2))))");
            Assert.AreEqual("-2", p.ToString());
            p = new BigIntegerRPNParser().Parse("-(-(-((2))))");
            Assert.AreEqual("-2", p.ToString());
            p = new BigIntegerRPNParser().Parse("(((((2) + (2)))))");
            Assert.AreEqual("4", p.ToString());
        }
        [TestMethod]
        public void ManySimpleTests()
        {
            var p = new BigIntegerRPNParser().Parse("2 * -2");
            Assert.AreEqual("-4", p.ToString());
            p = new BigIntegerRPNParser().Parse("2 * -2");
            Assert.AreEqual("-4", p.ToString());
            p = new BigIntegerRPNParser().Parse("2 + 2 * 2");
            Assert.AreEqual("6", p.ToString());
            p = new BigIntegerRPNParser().Parse("(2 + 2) * 2");
            Assert.AreEqual("8", p.ToString());
            p = new BigIntegerRPNParser().Parse("(-2 + 2) * 2");
            Assert.AreEqual("0", p.ToString());
            p = new BigIntegerRPNParser().Parse("8 * 3 % 5");
            Assert.AreEqual("4", p.ToString());
            p = new BigIntegerRPNParser().Parse("(2 / 3) * 7 + ((5 - 7) * (21 % (11/3)))");
            Assert.AreEqual("0", p.ToString());
        }
    }
}
