using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BignumArithmetics.BigFloatTests
{
    [TestClass]
    public class BigFloatIntListToStringTests
    {
        [TestMethod]
        public void NullTest()
        {
            string actual = BigFloat.IntListToString(null, 0);
            string expected = "";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EmptyTest()
        {
            string actual = BigFloat.IntListToString(new List<int>(), 0);
            string expected = "";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SimpleTest()
        {
            var list = new List<int> { 5 };
            string actual = BigFloat.IntListToString(list, 1);
            string expected = "5";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Num56Dot7_Test()
        {
            var list = new List<int> { 7, 6, 5 };
            string actual = BigFloat.IntListToString(list, 2);
            string expected = "56.7";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Num0Dot15_Test()
        {
            var list = new List<int> { 5, 1, 0 };
            string actual = BigFloat.IntListToString(list, 1);
            string expected = "0.15";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Num0598Dot20836610_Test()
        {
            var list = new List<int> { 0, 1, 6, 6, 3, 8, 0, 2, 8, 9, 5, 0 };
            string actual = BigFloat.IntListToString(list, 4);
            string expected = "0598.20836610";
            Assert.AreEqual(expected, actual);
        }
    }
}
