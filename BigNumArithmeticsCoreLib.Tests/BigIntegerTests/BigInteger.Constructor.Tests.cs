using System;
using NUnit.Framework;

namespace BignumArithmetics.BigIntegerTests
{
    [TestFixture]
    public class BigIntegerConstructorTests
    {
        [Test]
        public void NullString_In_Constructor()
        {
            Assert.Throws<ArgumentException>(() => new BigInteger((string)null));
        }

        [Test]
        public void NumberWithoutInteger_In_Constructor()
        {
            Assert.Throws<ArgumentException>(() => new BigInteger(" +.5"));
        }

        [Test]
        public void NumberWithDotWithoutFrac_In_Constructor()
        {
            Assert.Throws<ArgumentException>(() => new BigInteger("1234."));
        }

        [Test]
        public void NumWithTwoDots_In_Constructor()
        {
            Assert.Throws<ArgumentException>(() => new BigInteger("1.14.5"));
        }

        [Test]
        public void NumberWithAlpha_In_Constructor()
        {
            Assert.Throws<ArgumentException>(() => new BigInteger("  1a12.3 "));
        }

        [Test]
        public void Zero_In_Constructor()
        {
            BigInteger actual = new BigInteger("   0     ");

            Assert.AreEqual("0", actual.ToString());
            Assert.AreEqual(1, actual.Sign);
        }

        [Test]
        public void PlusZero_In_Constructor()
        {
            BigInteger actual = new BigInteger(" +0");

            Assert.AreEqual("0", actual.ToString());
            Assert.AreEqual(1, actual.Sign);
        }
        
        [Test]
        public void MinusZero_In_Constructor()
        {
            BigInteger actual = new BigInteger(" -0");

            Assert.AreEqual("0", actual.ToString());
            Assert.AreEqual(1, actual.Sign);
        }

        [Test]
        public void Plus123_In_Constructor()
        {
            BigInteger actual = new BigInteger(" +123");

            Assert.AreEqual("123", actual.ToString());
            Assert.AreEqual(1, actual.Sign);
        }

        [Test]
        public void Minus17_In_Constructor()
        {
            BigInteger actual = new BigInteger(" -17    ");

            Assert.AreEqual("-17", actual.ToString());
            Assert.AreEqual(-1, actual.Sign);
        }

        [Test]
        public void RandomBigNumber_In_Constructor()
        {
            BigInteger actual = new BigInteger("    -64737942379424723984729");

            Assert.AreEqual("-64737942379424723984729", actual.ToString());
            Assert.AreEqual(-1, actual.Sign);
        }

        [Test]
        public void ManyZeros_ToString()
        {
            BigInteger actual = new BigInteger("-0000");

            Assert.AreEqual("0", actual.ToString());
            Assert.AreEqual(1, actual.Sign);
        }

        [Test]
        public void ManyZerosDotManyZeros_ToString()
        {
            Assert.Throws<ArgumentException>(() => new BigInteger("0000.000"));
        }

        [Test]
        public void PlusHeadingZeros_ToString()
        {
            BigInteger actual = new BigInteger("+00001234");

            Assert.AreEqual("1234", actual.ToString());
            Assert.AreEqual(1, actual.Sign);
        }

        [Test]
        public void MinusHeadingZeros_ToString()
        { 
            BigInteger actual = new BigInteger("-000012340024823000");

            Assert.AreEqual("-12340024823000", actual.ToString());
            Assert.AreEqual(-1, actual.Sign);
        }

        [Test]
        public void Minus0Dot5_ToString()
        {
            Assert.Throws<ArgumentException>(() => new BigInteger("-0.5"));
        }
    }
}
