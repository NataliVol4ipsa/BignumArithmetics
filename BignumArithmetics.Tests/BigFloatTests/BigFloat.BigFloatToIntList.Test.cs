using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BignumArithmetics.BigFloatTests
{
    [TestClass]
    public class BigFloatBigFloatToIntListTest
    {
        public void CompareLists(List<int> expected, List<int>actual)
        {
            Assert.AreEqual(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        public void NullTest()
        {
            List<int> actual = BigFloat.BigFloatToIntList(null, 0, 0);
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void Zero_Test()
        {
            var bf = BigFloat.CreateFromString("0");
            List<int> actual = BigFloat.BigFloatToIntList(bf, 0, 0);
            List<int> expected = new List<int> { 0 };
            CompareLists(expected, actual);
        }
        [TestMethod]
        public void Zero23_Test()
        {
            var bf = BigFloat.CreateFromString("0");
            List<int> actual = BigFloat.BigFloatToIntList(bf, 2, 3);
            List<int> expected = new List<int> { 0, 0, 0, 0, 0 };
            CompareLists(expected, actual);
        }

        [TestMethod]
        public void Digit23_Test()
        {
            var bf = BigFloat.CreateFromString("5");
            List<int> actual = BigFloat.BigFloatToIntList(bf, 2, 3);
            List<int> expected = new List<int> { 0, 0, 0, 5, 0 };
            CompareLists(expected, actual);
        }

        [TestMethod]
        public void Digits32_Test()
        {
            var bf = BigFloat.CreateFromString("65.1");
            List<int> actual = BigFloat.BigFloatToIntList(bf, 3, 2);
            List<int> expected = new List<int> { 0, 1, 5, 6, 0 };
            CompareLists(expected, actual);
        }

        [TestMethod]
        public void Digits13_Test()
        {
            var bf = BigFloat.CreateFromString("65.1");
            List<int> actual = BigFloat.BigFloatToIntList(bf, 1, 3);
            List<int> expected = new List<int> { 0, 0, 1, 5, 6 };
            CompareLists(expected, actual);
        }
    }
}
