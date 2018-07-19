using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BignumArithmetics.BigFloatTests
{
    [TestClass]
    public class BigFloatNormalizeListTests
    {
        public void CompareLists(List<int> expected, List<int> actual)
        {
            Assert.AreEqual(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        public void NullTest()
        {
            new BigFloat().NormalizeList(null);
        }

        [TestMethod]
        public void OneElemList_Test()
        {
            var actual = new List<int> { 5 };
            var expected = new List<int> { 5 };
            new BigFloat().NormalizeList(actual);
            CompareLists(expected, actual);
        }

        [TestMethod]
        public void OneElemList_Unnormalized_Test()
        {
            var actual = new List<int> { 15 };
            var expected = new List<int> { 5, 1 };
            new BigFloat().NormalizeList(actual);
            CompareLists(expected, actual);
        }

        [TestMethod]
        public void OneElemList_VeryUnnormalized_Test()
        {
            var actual = new List<int> { 135673 };
            var expected = new List<int> { 3, 7, 6, 5, 3, 1 };
            new BigFloat().NormalizeList(actual);
            CompareLists(expected, actual);
        }

        [TestMethod]
        public void BigList_Unnormalized_Test()
        {
            var actual = new List<int> { 1, 11, 9 };
            var expected = new List<int> { 1, 1, 0, 1 };
            new BigFloat().NormalizeList(actual);
            CompareLists(expected, actual);
        }
    }
}
