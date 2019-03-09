using System;
using NUnit.Framework;

namespace BignumArithmetics.BigDecimalTests
{
    [TestFixture]
    public class BigDecimalConstructorTests
    {
        [Test]
        public void NullString_In_Constructor()
        {
            Assert.Throws<ArgumentException>(() => new BigDecimal((string)null));
        }

        [Test]
        public void NumberWithoutInteger_In_Constructor()
        {
            Assert.Throws<ArgumentException>(() => new BigDecimal(" +.5"));
        }

        [Test]
        public void NumberWithDotWithoutFrac_In_Constructor()
        {
            Assert.Throws<ArgumentException>(() => new BigDecimal("1234."));
        }

        [Test]
        public void NumWithTwoDots_In_Constructor()
        {
            Assert.Throws<ArgumentException>(() => new BigDecimal("1.14.5"));
        }

        [Test]
        public void NumberWithAlpha_In_Constructor()
        {
            Assert.Throws<ArgumentException>(() => new BigDecimal("  1a12.3 "));
        }

        [Test]
        public void Zero_In_Constructor()
        {
            BigDecimal actual = new BigDecimal("   0     ");

            Assert.AreEqual("0", actual.ToString());
            Assert.AreEqual(1, actual.Sign);
        }

        [Test]
        public void PlusZero_In_Constructor()
        {
            BigDecimal actual = new BigDecimal(" +0");

            Assert.AreEqual("0", actual.ToString());
            Assert.AreEqual(1, actual.Sign);
        }
        
        [Test]
        public void MinusZero_In_Constructor()
        {
            BigDecimal actual = new BigDecimal(" -0");

            Assert.AreEqual("0", actual.ToString());
            Assert.AreEqual(1, actual.Sign);
        }

        [Test]
        public void Plus12Dot3_In_Constructor()
        {
            BigDecimal actual = new BigDecimal(" +12.3");

            Assert.AreEqual("12.3", actual.ToString());
            Assert.AreEqual(1, actual.Sign);
        }

        [Test]
        public void Minus17_In_Constructor()
        {
            BigDecimal actual = new BigDecimal(" -17    ");

            Assert.AreEqual("-17", actual.ToString());
            Assert.AreEqual(-1, actual.Sign);
        }

        [Test]
        public void RandomBigNumber_In_Constructor()
        {
            BigDecimal actual = new BigDecimal("    -6473794237942.4723984729");

            Assert.AreEqual("-6473794237942.4723984729", actual.ToString());
            Assert.AreEqual(-1, actual.Sign);
        }

        [Test]
        public void Number_12Dot0_In_Constructor()
        {
            BigDecimal actual = new BigDecimal("12.0");

            Assert.AreEqual("12", actual.ToString());
            Assert.AreEqual(1, actual.Sign);
        }

        [Test]
        public void Plus_12Dot0_ToString()
        {
            BigDecimal actual = new BigDecimal("+12.0");

            Assert.AreEqual("12", actual.ToString());
            Assert.AreEqual(1, actual.Sign);
        }

        [Test]
        public void ManyZeros_ToString()
        {
            BigDecimal actual = new BigDecimal("-0000");

            Assert.AreEqual("0", actual.ToString());
            Assert.AreEqual(1, actual.Sign);
        }

        [Test]
        public void ManyZerosDotManyZeros_ToString()
        {
            BigDecimal actual = new BigDecimal("0000.000");

            Assert.AreEqual("0", actual.ToString());
            Assert.AreEqual(1, actual.Sign);
        }

        [Test]
        public void PlusHeadingZeros_ToString()
        {
            BigDecimal actual = new BigDecimal("+00001234");

            Assert.AreEqual("1234", actual.ToString());
            Assert.AreEqual(1, actual.Sign);
        }

        [Test]
        public void MinusHeadingZeros_ToString()
        {
            BigDecimal actual = new BigDecimal("-00001234");

            Assert.AreEqual("-1234", actual.ToString());
            Assert.AreEqual(-1, actual.Sign);
        }

        [Test]
        public void MinusHeadingAndTailingZeros_ToString()
        {
            BigDecimal actual = new BigDecimal("-00001234.0024823000");

            Assert.AreEqual("-1234.0024823", actual.ToString());
            Assert.AreEqual(-1, actual.Sign);
        }

        [Test]
        public void Minus0Dot5_ToString()
        {
            BigDecimal actual = new BigDecimal("-0.5");

            Assert.AreEqual("-0.5", actual.ToString());
            Assert.AreEqual(-1, actual.Sign);
        }

        [Test]
        public void Minus000Dot5000_ToString()
        {
            BigDecimal actual = new BigDecimal("-000.5000");

            Assert.AreEqual("-0.5", actual.ToString());
            Assert.AreEqual(-1, actual.Sign);
        }
    }
}
