using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BignumArithmetics.BigIntegerTests
{
    [TestClass]
    public class BigIntegerConstructorTests
    {
        [TestMethod]
        public void NullString_In_Constructor()
        {
            BigInteger empty = BigInteger.CreateFromString((string)null);

            Assert.AreEqual("0", empty.ToString());
            Assert.AreEqual(1, empty.Sign);
        }

        [TestMethod]
        public void NumberWithoutInteger_In_Constructor()
        {
            BigInteger actual = BigInteger.CreateFromString(" +.5");

            Assert.AreEqual("0", actual.ToString());
            Assert.AreEqual(1, actual.Sign);
        }

        [TestMethod]
        public void NumberWithDotWithoutFrac_In_Constructor()
        {
            BigInteger actual = BigInteger.CreateFromString("1234.");

            Assert.AreEqual("0", actual.ToString());
            Assert.AreEqual(1, actual.Sign);
        }

        [TestMethod]
        public void NumWithTwoDots_In_Constructor()
        {
            BigInteger actual = BigInteger.CreateFromString("1.14.5");

            Assert.AreEqual("0", actual.ToString());
            Assert.AreEqual(1, actual.Sign);
        }

        [TestMethod]
        public void NumberWithAlpha_In_Constructor()
        {
            BigInteger actual = BigInteger.CreateFromString("  1a12.3 ");

            Assert.AreEqual("0", actual.ToString());
            Assert.AreEqual(1, actual.Sign);
        }

        [TestMethod]
        public void Zero_In_Constructor()
        {
            BigInteger actual = BigInteger.CreateFromString("   0     ");

            Assert.AreEqual("0", actual.ToString());
            Assert.AreEqual(1, actual.Sign);
        }

        [TestMethod]
        public void PlusZero_In_Constructor()
        {
            BigInteger actual = BigInteger.CreateFromString(" +0");

            Assert.AreEqual("0", actual.ToString());
            Assert.AreEqual(1, actual.Sign);
        }
        
        [TestMethod]
        public void MinusZero_In_Constructor()
        {
            BigInteger actual = BigInteger.CreateFromString(" -0");

            Assert.AreEqual("0", actual.ToString());
            Assert.AreEqual(1, actual.Sign);
        }

        [TestMethod]
        public void Plus123_In_Constructor()
        {
            BigInteger actual = BigInteger.CreateFromString(" +123");

            Assert.AreEqual("123", actual.ToString());
            Assert.AreEqual(1, actual.Sign);
        }

        [TestMethod]
        public void Minus17_In_Constructor()
        {
            BigInteger actual = BigInteger.CreateFromString(" -17    ");

            Assert.AreEqual("-17", actual.ToString());
            Assert.AreEqual(-1, actual.Sign);
        }

        [TestMethod]
        public void RandomBigNumber_In_Constructor()
        {
            BigInteger actual = BigInteger.CreateFromString("    -64737942379424723984729");

            Assert.AreEqual("-64737942379424723984729", actual.ToString());
            Assert.AreEqual(-1, actual.Sign);
        }

        [TestMethod]
        public void ManyZeros_ToString()
        {
            BigInteger actual = BigInteger.CreateFromString("-0000");

            Assert.AreEqual("0", actual.ToString());
            Assert.AreEqual(1, actual.Sign);
        }

        [TestMethod]
        public void ManyZerosDotManyZeros_ToString()
        {
            BigInteger actual = BigInteger.CreateFromString("0000.000");

            Assert.AreEqual("0", actual.ToString());
            Assert.AreEqual(1, actual.Sign);
        }

        [TestMethod]
        public void PlusHeadingZeros_ToString()
        {
            BigInteger actual = BigInteger.CreateFromString("+00001234");

            Assert.AreEqual("1234", actual.ToString());
            Assert.AreEqual(1, actual.Sign);
        }

        [TestMethod]
        public void MinusHeadingZeros_ToString()
        { 
            BigInteger actual = BigInteger.CreateFromString("-000012340024823000");

            Assert.AreEqual("-12340024823000", actual.ToString());
            Assert.AreEqual(-1, actual.Sign);
        }

        [TestMethod]
        public void Minus0Dot5_ToString()
        {
            BigInteger actual = BigInteger.CreateFromString("-0.5");

            Assert.AreEqual("0", actual.ToString());
            Assert.AreEqual(1, actual.Sign);
        }
    }
}
