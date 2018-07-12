using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace net.NataliVol4ica.BignumArithmetics.Tests
{
    [TestClass]
    public class FixedPointNumberConstructorTests
    {
        [TestMethod]
        [ExpectedException(typeof(NumberFormatException),
        "Cannot create FixedPointNumber of \"null\"")]
        public void NullString_In_Constructor()
        {
            BigFloat empty = new BigFloat(null);
        }

        [TestMethod]
        [ExpectedException(typeof(NumberFormatException),
        "Cannot create FixedPointNumber of \" +.5\"")]
        public void NumberWithoutInteger_In_Constructor()
        {
            BigFloat actual = new BigFloat(" +.5");
        }

        [TestMethod]
        [ExpectedException(typeof(NumberFormatException),
        "Cannot create FixedPointNumber of \"1234.\"")]
        public void NumberWithDotWithoutFrac_In_Constructor()
        {
            BigFloat actual = new BigFloat("1234.");
        }

        [TestMethod]
        [ExpectedException(typeof(NumberFormatException),
        "Cannot create FixedPointNumber of \"1.14.5\"")]
        public void NumWithTwoDots_In_Constructor()
        {
            BigFloat actual = new BigFloat("1.14.5");
        }

        [TestMethod]
        [ExpectedException(typeof(NumberFormatException),
        "Cannot create FixedPointNumber of \"  1a12.3 \"")]
        public void NumberWithAlpha_In_Constructor()
        {
            BigFloat actual = new BigFloat("  1a12.3 ");
        }

        [TestMethod]
        public void Zero_In_Constructor()
        {
            BigFloat actual = new BigFloat("   0     ");

            Assert.AreEqual("0", actual.ToString());
            Assert.AreEqual(1, actual.Sign);
        }

        [TestMethod]
        public void PlusZero_In_Constructor()
        {
            BigFloat actual = new BigFloat(" +0");

            Assert.AreEqual("0", actual.ToString());
            Assert.AreEqual(1, actual.Sign);
        }
        
        [TestMethod]
        public void MinusZero_In_Constructor()
        {
            BigFloat actual = new BigFloat(" -0");

            Assert.AreEqual("0", actual.ToString());
            Assert.AreEqual(1, actual.Sign);
        }

        [TestMethod]
        public void Plus12Dot3_In_Constructor()
        {
            BigFloat actual = new BigFloat(" +12.3");

            Assert.AreEqual("12.3", actual.ToString());
            Assert.AreEqual(1, actual.Sign);
        }

        [TestMethod]
        public void Minus17_In_Constructor()
        {
            BigFloat actual = new BigFloat(" -17    ");

            Assert.AreEqual("-17", actual.ToString());
            Assert.AreEqual(-1, actual.Sign);
        }

        [TestMethod]
        public void RandomBigNumber_In_Constructor()
        {
            BigFloat actual = new BigFloat("    -6473794237942.4723984729");

            Assert.AreEqual("-6473794237942.4723984729", actual.ToString());
            Assert.AreEqual(-1, actual.Sign);
        }

        [TestMethod]
        public void Number_12Dot0_In_Constructor()
        {
            BigFloat actual = new BigFloat("12.0");

            Assert.AreEqual("12", actual.ToString());
            Assert.AreEqual(1, actual.Sign);
        }

        [TestMethod]
        public void Plus_12Dot0_ToString()
        {
            BigFloat actual = new BigFloat("+12.0");

            Assert.AreEqual("12", actual.ToString());
            Assert.AreEqual(1, actual.Sign);
        }

        [TestMethod]
        public void ManyZeros_ToString()
        {
            BigFloat actual = new BigFloat("-0000");

            Assert.AreEqual("0", actual.ToString());
            Assert.AreEqual(1, actual.Sign);
        }

        [TestMethod]
        public void ManyZerosDotManyZeros_ToString()
        {
            BigFloat actual = new BigFloat("0000.000");

            Assert.AreEqual("0", actual.ToString());
            Assert.AreEqual(1, actual.Sign);
        }

        [TestMethod]
        public void PlusHeadingZeros_ToString()
        {
            BigFloat actual = new BigFloat("+00001234");

            Assert.AreEqual("1234", actual.ToString());
            Assert.AreEqual(1, actual.Sign);
        }

        [TestMethod]
        public void MinusHeadingZeros_ToString()
        {
            BigFloat actual = new BigFloat("-00001234");

            Assert.AreEqual("-1234", actual.ToString());
            Assert.AreEqual(-1, actual.Sign);
        }

        [TestMethod]
        public void MinusHeadingAndTailingZeros_ToString()
        {
            BigFloat actual = new BigFloat("-00001234.0024823000");

            Assert.AreEqual("-1234.0024823", actual.ToString());
            Assert.AreEqual(-1, actual.Sign);
        }

        [TestMethod]
        public void Minus0Dot5_ToString()
        {
            BigFloat actual = new BigFloat("-0.5");

            Assert.AreEqual("-0.5", actual.ToString());
            Assert.AreEqual(-1, actual.Sign);
        }

        [TestMethod]
        public void Minus000Dot5000_ToString()
        {
            BigFloat actual = new BigFloat("-000.5000");

            Assert.AreEqual("-0.5", actual.ToString());
            Assert.AreEqual(-1, actual.Sign);
        }
    }
}
