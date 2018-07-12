using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BignumArithmetics.Tests
{
    [TestClass]
    public class BigNumberTests
    {
        [TestMethod]
        public void ToChar_Whole()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = -2; i < 18; i++)
                sb.Append(BigFloat.ToChar(i));
            Assert.AreEqual("000123456789abcdef00", sb.ToString());
        }
        [TestMethod]
        public void ToDigit_Whole()
        {
            string numbers = "0123456789abcdefABCDEFqw`\ng";
            int[] expected = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 10, 11, 12, 13, 14, 15, -1, -1, -1, -1, -1 };
            for (int i = 0; i < numbers.Length; i++)
                Assert.AreEqual(expected[i], BigNumber.ToDigit(numbers[i]));
        }
    }
}
