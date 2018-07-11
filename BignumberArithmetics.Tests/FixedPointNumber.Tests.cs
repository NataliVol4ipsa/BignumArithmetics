using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace net.NataliVol4ica.BignumArithmetics.Tests
{
    //TODO: RENAME METHODS
    [TestClass]
    public class FixedPointNumberTests
    {
        [TestMethod]
        [ExpectedException(typeof(NumberFormatException),
        "Cannot create FixedPointNumber of \"null\"")]
        public void NullStringInConstructor()
        {
            FixedPointNumber empty = new FixedPointNumber(null);
        }

        [TestMethod]
        [ExpectedException(typeof(NumberFormatException),
        "Cannot create FixedPointNumber of \" +.5\"")]
        public void IncorrectStringInConstructor_1()
        {
            FixedPointNumber actual = new FixedPointNumber(" +.5");
        }

        [TestMethod]
        [ExpectedException(typeof(NumberFormatException),
        "Cannot create FixedPointNumber of \"1234.\"")]
        public void IncorrectStringInConstructor_2()
        {
            FixedPointNumber actual = new FixedPointNumber("1234.");
        }

        [TestMethod]
        [ExpectedException(typeof(NumberFormatException),
        "Cannot create FixedPointNumber of \"1.14.5\"")]
        public void IncorrectStringInConstructor_3()
        {
            FixedPointNumber actual = new FixedPointNumber("1.14.5");
        }

        [TestMethod]
        [ExpectedException(typeof(NumberFormatException),
        "Cannot create FixedPointNumber of \"  1a12.3 \"")]
        public void IncorrectStringInConstructor_4()
        {
            FixedPointNumber actual = new FixedPointNumber("  1a12.3 ");
        }

        [TestMethod]
        public void CorrectStringInConstructor_1()
        {
            FixedPointNumber actual = new FixedPointNumber("   0     ");

            Assert.AreEqual("0", actual.ToString());
        }

        [TestMethod]
        public void CorrectStringInConstructor_2()
        {
            FixedPointNumber actual = new FixedPointNumber(" +0");

            Assert.AreEqual("0", actual.ToString());
        }

        [TestMethod]
        public void CorrectStringInConstructor_3()
        {
            FixedPointNumber actual = new FixedPointNumber(" -0");

            Assert.AreEqual("0", actual.ToString());
        }

        [TestMethod]
        public void CorrectStringInConstructor_4()
        {
            FixedPointNumber actual = new FixedPointNumber(" +12.3");

            Assert.AreEqual("12.3", actual.ToString());
        }

        [TestMethod]
        public void CorrectStringInConstructor_5()
        {
            FixedPointNumber actual = new FixedPointNumber(" -17    ");

            Assert.AreEqual("-17", actual.ToString());
        }

        [TestMethod]
        public void CorrectStringInConstructor_6()
        {
            FixedPointNumber actual = new FixedPointNumber("    -6473794237942.4723984729");

            Assert.AreEqual("-6473794237942.4723984729", actual.ToString());
        }

        [TestMethod]
        public void CorrectStringInConstructor_7()
        {
            FixedPointNumber actual = new FixedPointNumber("12.0");

            Assert.AreEqual("12.0", actual.ToString());
        }

    }
}
