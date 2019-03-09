using System.Collections.Generic;
using NUnit.Framework;

namespace BignumArithmetics.BigDecimalTests
{
    [TestFixture]
    public class BigDecimalNormalizeListTests
    {
        public void CompareLists(List<int> expected, List<int> actual)
        {
            Assert.AreEqual(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [Test]
        public void NullTest()
        {
            new BigDecimal().NormalizeList(null);
        }

        [Test]
        public void OneElemList_Test()
        {
            var actual = new List<int> { 5 };
            var expected = new List<int> { 5 };
            new BigDecimal().NormalizeList(actual);
            CompareLists(expected, actual);
        }

        [Test]
        public void OneElemList_Unnormalized_Test()
        {
            var actual = new List<int> { 15 };
            var expected = new List<int> { 5, 1 };
            new BigDecimal().NormalizeList(actual);
            CompareLists(expected, actual);
        }

        [Test]
        public void OneElemList_VeryUnnormalized_Test()
        {
            var actual = new List<int> { 135673 };
            var expected = new List<int> { 3, 7, 6, 5, 3, 1 };
            new BigDecimal().NormalizeList(actual);
            CompareLists(expected, actual);
        }

        [Test]
        public void BigList_Unnormalized_Test()
        {
            var actual = new List<int> { 1, 11, 9 };
            var expected = new List<int> { 1, 1, 0, 1 };
            new BigDecimal().NormalizeList(actual);
            CompareLists(expected, actual);
        }
    }
}
