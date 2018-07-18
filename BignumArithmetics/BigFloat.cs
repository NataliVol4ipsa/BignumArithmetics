using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

namespace BignumArithmetics
{
    /// <summary>Сlass for fixed point big numbers</summary>
    public class BigFloat : BigNumber
    {
        #region Constructors
        /// <summary>Constructor creates a BigFLoat equal to 0</summary>
        /// <returns>An instance of BigFloat</returns>
        public BigFloat()
        {
            CleanString = "0";
        }
        //todo: REMAKE IT TO CALL COPY()  ?
        /// <summary>Constructor creates a BigFloat equal to parameter</summary>
        /// <param name="from">any BigFloat</param>
        /// <returns>An instance of BigFloat equal to parameter</returns>
        public BigFloat(BigFloat from)
        {
            CleanString = from.CleanString;
            if (from.Sign < 0)
                SwitchSign();
            DotPos = from.DotPos;
            Fractional = from.Fractional;
        }
        /// <summary>Private constructor creates a BigFloat from a valid string 
        /// that is matching <see cref="validStringRegEx"/> 
        /// and is cut with <see cref="cleanStringRegEx"/></summary>
        /// <param name="str">string representing number digits and delimiter</param>
        /// <param name="sign">integer representing number sign</param>
        /// <returns>An instance of BigFloat</returns>
        private BigFloat(string str, int sign)
        {
            CleanString = str;
            if (sign < 0)
                SwitchSign();
        }
        #endregion

        #region Static Methods
        /// <summary>Fabric thar returns an instance of BigFloat constructed from a string
        /// that is matching <see cref="validStringRegEx"/> 
        /// and is cut with <see cref="cleanStringRegEx"/></summary>
        /// <param name="str">String that represents a number</param>
        /// <returns>An instance of BigFloat. null if parameter is invalid</returns>
        public static BigFloat CreateFromString(string str)
        {
            if (string.IsNullOrEmpty(str) ||
                !Regex.IsMatch(str, validStringRegEx, RegexOptions.None))
                return new BigFloat();
            str = CleanNumericString(str, out int sign);
            return new BigFloat(str, sign);
        }
             /// <summary>Fabric thar returns an instance of BigFloat constructed from a number</summary>
        /// <param name="number">Object of any numeric type</param>
        /// <returns>An instance of BigFloat. null if parameter is invalid</returns>
        public static BigFloat CreateFromNumber<T>(T number)
        {
            string str = number.ToString();
            string sysDelim = System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            str = str.Replace(sysDelim, delimiter);
            return CreateFromString(str);
        }
        //TODO: make these virtual\override
        /// <summary>Сonverts BigFloat into reversed digit list with padding zeroes</summary>
        /// <param name="desiredInt">int represening how many digits should integer part contain</param>
        /// <param name="desiredFrac">int represening how many digits should fractional part contain</param>
        /// <returns>List of digits</returns>
        public static List<int> BigNumberToIntList(BigFloat num, int desiredInt = 0, int desiredFrac = 0)
        {
            if (num is null)
                return null;

            List<int> ret = new List<int>();
            int IntZeros, FracZeros;

            IntZeros = Math.Max(num.Integer, desiredInt) - num.Integer;
            FracZeros = Math.Max(num.Fractional, desiredFrac) - num.Fractional;
            ret.AddRange(Enumerable.Repeat(0, FracZeros));
            for (int i = num.CleanString.Length - 1; i >= 0; i--)
                if (num.CleanString[i] != '.')
                    ret.Add(ToDigit(num.CleanString[i]));
            ret.AddRange(Enumerable.Repeat(0, IntZeros));
            return ret;
        }
        /// <summary>Normalizes reversed list of digits</summary>
        /// <param name="digits">List of digits</param>
        public static void NormalizeList(List<int> digits)
        {
            int i;

            if (digits is null || digits.Count == 0)
                return;
            for (i = 0; i < digits.Count - 1; i++)
            {
                if (digits[i] < 0)
                {
                    //todo: if i + 1 >= Count then error
                    digits[i] += 10;
                    digits[i + 1]--;
                }
                else
                {
                    digits[i + 1] += digits[i] / 10;
                    digits[i] %= 10;
                }
            }
            while (digits[i] > 9)
            {
                digits.Add(digits[i] / 10);
                digits[i] %= 10;
                i++;
            }
        }
        /// <summary>IntListToString method converts digit list to string</summary>
        /// <param name="digits">List of digits</param>
        /// <param name="dotPos">Integer representing position of dot in string</param>
        /// <returns>A string representing a number; null in case of invalid arguments</returns>
        public static string IntListToString(List<int> digits, int dotPos = 0)
        {
            int i;
            int reverseDot;
            StringBuilder sb;

            if (digits is null || digits.Count == 0)
                return "";
            if (dotPos < 0)
                dotPos = 0;
            sb = new StringBuilder();
            reverseDot = digits.Count - dotPos;
            for (i = digits.Count - 1; i >= reverseDot; i--)
                sb.Append(ToChar(digits[i]));
            if (i >= 0)
            {
                sb.Append(delimiter);
                while (i >= 0)
                    sb.Append(ToChar(digits[i--]));
            }
            return sb.ToString();
        }

        /// <summary>CleanNumericString method cleans digit string with <see cref="cleanStringRegEx"/></summary>
        /// <param name="RawString">String representing of digits</param>
        /// <param name="sign">Integer representing position of dot in cleaned string</param>
        /// <returns>A clean string; "0" in case of invalid arguments</returns>
        private static string CleanNumericString(string RawString, out int sign)
        {
            string substr;

            if (RawString is null)
            {
                sign = 1;
                return "0";
            }
            substr = Regex.Match(RawString, cleanStringRegEx).Value;
            if (substr == "")
            {
                sign = 1;
                return "0";
            }
            sign = RawString.Contains("-") ? -1 : 1;
            return substr;
        }
        #endregion

        #region Private Methods
        /// <summary>Calculates a position of delimiter in <see cref="CleanString"/></summary>
        private void FindDotPos()
        {
            if (_dotPos > 0)
                return;
            _dotPos = CleanString.IndexOf(delimiter);
            if (_dotPos < 0)
                _dotPos = CleanString.Length;
        }
        /// <summary> Generates a list representing sum of two reversed digit lists</summary>
        /// <param name="leftList">First operand</param>
        /// <param name="rightList">Second operand</param>
        /// <param name="toNorm">Shows if the <see cref="NormalizeList(List{int})"/>
        /// should be called for result</param>
        /// <returns>Sum of two lists in same reversed form</returns>
        private static List<int> SumTwoLists(List<int> leftList, List<int> rightList, bool toNorm = true)
        {
            if (leftList.Count <= 0 || rightList.Count <= 0)
                return new List<int>();
            int maxlen = Math.Max(leftList.Count, rightList.Count);
            leftList.AddRange(Enumerable.Repeat(0, maxlen - leftList.Count));
            rightList.AddRange(Enumerable.Repeat(0, maxlen - rightList.Count));
            var resultList = new List<int>(leftList.Count);
            for (int i = 0; i < maxlen; i++)
                resultList.Add(leftList[i] + rightList[i]);
            if (toNorm)
                NormalizeList(resultList);
            return resultList;
        }
        /// <summary> Generates a list representing substraction of two reversed digit lists.</summary>
        /// <param name="leftList">First list.
        /// Number represented by leftList has to be bigger than the one by rightList!</param>
        /// <param name="rightList">Second list.</param>
        /// <param name="toNorm">Shows if the <see cref="NormalizeList(List{int})"/>
        /// should be called for result</param>
        /// <returns>Substraction of two lists in same reversed form</returns>
        /// <exception cref="ArgumentException">Exception thrown if leftList < rightList</exception>
        private static List<int> DifTwoLists(List<int> leftList, List<int> rightList, bool toNorm = true)
        {
            if (CompareLists(leftList, rightList) < 0)
                throw new ArgumentException("leftList cannot be bigger than rightList!");
            rightList.AddRange(Enumerable.Repeat(0, leftList.Count - rightList.Count));            
            var resultList = new List<int>();
            for (int i = 0; i < leftList.Count; i++)
                resultList.Add(leftList[i] - rightList[i]);
            if (toNorm)
                NormalizeList(resultList);
            return resultList;
        }
        /// <summary> Generates a list representing multiplication of two reversed digit lists.</summary>
        /// <param name="leftList">First list.</param>
        /// <param name="rightList">Second list.</param>
        /// <param name="toNorm">Shows if the <see cref="NormalizeList(List{int})"/>
        /// should be called for result</param>
        /// <returns>Multiplication of two lists in same reversed form</returns>
        private static List<int> MulTwoLists(List<int> leftList, List<int> rightList, bool toNorm = false)
        {
            var resultList = new List<int>();
            var tempList = new List<int>();
            resultList = MulListAndDigit(leftList, rightList[0], false);
            for (int i = 1; i < rightList.Count; i++)
            {
                if (rightList[i] == 0)
                    continue;
                tempList = MulListAndDigit(leftList, rightList[i], false, i);
                resultList = SumTwoLists(resultList, tempList, false);
            }
            if (toNorm)
                NormalizeList(resultList);
            return resultList;
        }
        //todo: remove unnecessary reverses!!
        //todo: make it take precision as argument?
        private static List<int> DivTwoLists(List<int> leftList, List<int> rightList, out List<int> remainder)
        {
            int addedDigitsInARow = 0;
            var resultList = new List<int>();
            remainder = new List<int>();
            if (CompareLists(leftList, rightList) >= 0)
            {
                bool unnormed = true;
                int sum, dif;
                int toAdd = rightList.Count;
                for (int i = 0; i < toAdd; i++)
                    remainder.Add(leftList[i]);
                addedDigitsInARow = 0;
                do
                {
                    if (unnormed)
                        while (remainder.Count > 0 && remainder[0] == 0)
                            remainder.RemoveAt(0);
                    unnormed = true;
                    sum = 0;
                    addedDigitsInARow++;
                    while (CompareLists(remainder, rightList) >= 0)
                    {
                        dif = remainder.Count - rightList.Count;
                        remainder.Reverse();
                        rightList.Reverse();
                        AddTailingZeros(rightList, dif);
                        remainder = DifTwoLists(remainder, rightList);
                        if (dif > 0)
                            rightList.RemoveAt(rightList.Count - 1);
                        while (remainder.Count > 0 && remainder.Last() == 0)
                            remainder.RemoveAt(remainder.Count - 1);
                        remainder.Reverse();
                        rightList.Reverse();
                        sum++;
                        unnormed = false;
                        addedDigitsInARow = 0;
                    }
                    resultList.Add(sum);
                    if (toAdd < leftList.Count)
                        remainder.Add(leftList[toAdd]);
                    toAdd++;
                } while (toAdd <= leftList.Count);
            }
            if (resultList.Count == 0)
                resultList.Add(0);
            return resultList;
        }
        private static List<int> MulListAndDigit(List<int> leftList, int digit, bool toNorm = true, int padding = 0)
        {
            if (digit == 0)
                return new List<int> { 0 };
            var resultList = new List<int>(leftList.Count);
            resultList.AddRange(Enumerable.Repeat(0, padding));
            for (int i = 0; i < leftList.Count; i++)
                resultList.Add(leftList[i] * digit);
            if (toNorm)
                NormalizeList(resultList);
            return resultList;
        }
        private static void RemoveTailingZeros(List<int> list)
        {
            while (list.Last() == 0 && list.Count > 1)
                list.RemoveAt(list.Count - 1);
        }
        private static void AddTailingZeros(List<int> list, int amount)
        {
            if (amount < 0)
                return;
            list.AddRange(Enumerable.Repeat(0, amount));
        }
        /// <summary> Compares two reversed list of digits 
        /// and returns an int representing their order when sorted </summary>
        /// <param name="left">First list to compare</param>
        /// <param name="right">Second list to compare</param>
        /// <returns>An int representing list order when sorted</returns>
        private static int CompareLists(List<int> left, List<int> right)
        {
            if (left.Count != right.Count)
                return left.Count - right.Count;
            int i = left.Count - 1;
            while (i >= 0 && left[i] == right[i])
                i++;
            if (i == -1)
                return 0;
            return left[i] - right[i];
        }
        #endregion

        #region Parent Overrides
        //todo: CHECK IF PARAMETERS ARE REALLY BIGFLOATS
        /// <summary>Method that is calculating (this + op) for BigFLoat objects.
        /// Overrided from parent</summary>
        /// <param name="op">Second operand</param>
        /// <returns>BigFloat equal to (this + op)  upcasted to BigNumber</returns>
        public override BigNumber Add(BigNumber op)
        {
            
            BigFloat bfLeft = this;
            BigFloat bfRight = (BigFloat)op;

            if (bfLeft.Sign != bfRight.Sign)
                return bfLeft.Substract(-bfRight);

            int desiredInt = Math.Max(bfLeft.Integer, bfRight.Integer);
            int desiredFrac = Math.Max(bfLeft.Fractional, bfRight.Fractional);
            var leftList = BigNumberToIntList(bfLeft, desiredInt, desiredFrac);
            var rightList = BigNumberToIntList(bfRight, desiredInt, desiredFrac);
            var resultList = SumTwoLists(leftList, rightList);

            BigFloat bfAns = CreateFromString(IntListToString(resultList, resultList.Count - desiredFrac));
            if (Sign < 0)
                bfAns.SwitchSign();
            return bfAns;
        }
        /// <summary>Method that is calculating (this - op) for BigFLoat objects.
        /// Overrided from parent</summary>
        /// <param name="op">Second operand</param>
        /// <returns>BigFloat equal to (this - op)  upcasted to BigNumber</returns>
        public override BigNumber Substract(BigNumber op)
        {
            BigFloat bfLeft = this;
            BigFloat bfRight = (BigFloat)op;

            if (bfLeft.Sign > 0 && bfRight.Sign < 0)
                return bfLeft.Add(-bfRight);
            if (bfLeft.Sign < 0 && bfRight.Sign > 0)
                return -(BigFloat)bfRight.Add(-bfLeft);
            if (bfLeft.Sign < 0 && bfRight.Sign < 0)
                return (-bfRight).Substract(-bfLeft);
            //both operands are > 0 here
            int sign = 1;

            if (bfLeft < bfRight)
            {
                sign = -sign;
                Swap(ref bfLeft, ref bfRight);
            }
            int desiredInt = Math.Max(bfLeft.Integer, bfRight.Integer);
            int desiredFrac = Math.Max(bfLeft.Fractional, bfRight.Fractional);
            var leftList = BigNumberToIntList(bfLeft, desiredInt, desiredFrac);
            var rightList = BigNumberToIntList(bfRight, desiredInt, desiredFrac);
            var resultList = DifTwoLists(leftList, rightList);
        
            BigFloat bfAns = CreateFromString(IntListToString(resultList, resultList.Count - desiredFrac));
            if (sign < 0)
                bfAns.SwitchSign();
            return bfAns;
        }
        /// <summary>Method that is calculating (this * op) for BigFLoat objects.
        /// Overrided from parent</summary>
        /// <param name="op">Second operand</param>
        /// <returns>BigFloat equal to (this * op)  upcasted to BigNumber</returns>
        public override BigNumber Multiply(BigNumber op)
        {
            BigFloat bfLeft = this;
            BigFloat bfRight = (BigFloat)op;
            
            if (bfLeft.Integer + bfLeft.Fractional < bfRight.Integer + bfRight.Fractional)
                Swap(ref bfLeft, ref bfRight);
            int newDot = bfLeft.Fractional + bfRight.Fractional;
            var leftList = BigNumberToIntList(bfLeft);
            var rightList = BigNumberToIntList(bfRight);
            var resultList = MulTwoLists(leftList, rightList);

            BigFloat bfAns = CreateFromString(IntListToString(resultList, resultList.Count - newDot));
            if (bfLeft.Sign * bfRight.Sign < 0)
                bfAns.SwitchSign();
            return bfAns;
        }
        /// <summary>Method that is calculating (this / op) for BigFLoat objects.
        /// Overrided from parent</summary>
        /// <param name="op">Second operand</param>
        /// <returns>BigFloat equal to (this / op)  upcasted to BigNumber</returns>
        public override BigNumber Divide(BigNumber op)
        {
            if (op.CleanString == "0")
                throw new DivideByZeroException();
            BigFloat bfLeft = this;
            BigFloat bfRight = (BigFloat)op;

            int multiplier = Math.Max(bfLeft.Fractional, bfRight.Fractional);
            var leftList = BigNumberToIntList(bfLeft, 0, multiplier);
            var rightList = BigNumberToIntList(bfRight, 0, multiplier);
            RemoveTailingZeros(leftList);
            RemoveTailingZeros(rightList);
            leftList.Reverse();
            rightList.Reverse();

            AddTailingZeros(leftList, FracPrecision);
            List<int> resultList = DivTwoLists(leftList, rightList, out List<int> subList);
            int dotPos = resultList.Count - FracPrecision;           
            resultList.Reverse();

            BigFloat bfAns = CreateFromString(IntListToString(resultList, dotPos));
            if (bfLeft.Sign * bfRight.Sign < 0)
                bfAns.SwitchSign();
            return bfAns;
        }
        /// <summary>Method that is calculating (this % op) for BigFLoat objects.
        /// Overrided from parent</summary>
        /// <param name="op">Second operand</param>
        /// <returns>BigFloat equal to (this % op)  upcasted to BigNumber</returns>
        public override BigNumber Mod(BigNumber op)
        {
            if (op.CleanString == "0")
                throw new ArgumentException("Cannot calculate BigFloat % 0");
            //div this by op until this >= op; return this;
            return new BigFloat();
        }
        #endregion

        #region Operators
        public static BigFloat operator -(BigFloat num)
        {
            BigFloat ret = new BigFloat(num);

            ret.SwitchSign();
            return ret;
        }

        public static BigFloat operator +(BigFloat left, BigFloat right)
        {
            if (left is null || right is null)
                return null;
            return (BigFloat)left.Add(right);
        }
        public static BigFloat operator -(BigFloat left, BigFloat right)
        {
            if (left is null || right is null)
                return null;
            return (BigFloat)left.Substract(right);
        }
        public static BigFloat operator *(BigFloat left, BigFloat right)
        {
            if (left is null || right is null)
                return null;
            return (BigFloat)left.Multiply(right);
        }
        public static BigFloat operator /(BigFloat left, BigFloat right)
        {
            if (left is null || right is null)
                return null;
            return (BigFloat)left.Divide(right);
        }
        public static BigFloat operator %(BigFloat left, BigFloat right)
        {
            if (left is null || right is null)
                return null;
            return (BigFloat)left.Mod(right);
        }
        public static bool operator >(BigFloat left, BigFloat right)
        {
            if (left.Integer > right.Integer)
                return true;
            if (left.Integer < right.Integer)
                return false;
            if (string.Compare(left.CleanString, right.CleanString) > 0)
                return true;
            return false;
        }
        public static bool operator <(BigFloat left, BigFloat right)
        {
            if (left.Integer > right.Integer)
                return false;
            if (left.Integer < right.Integer)
                return true;
            if (string.Compare(left.CleanString, right.CleanString) < 0)
                return true;
            return false;
        }
        #endregion

        #region Variables
        /// <summary>delimiter represents a symbol or string that splits number
        /// into integer and fractional parts </summary>
        private static readonly string delimiter = ".";
        /// <summary>validStringRegEx is a string representing RegEx
        /// used to validate input string in fabric method <see cref="CreateFromString"/>
        /// into integer and fractional parts </summary>
        private static readonly string validStringRegEx = @"^\s*[+-]?[0-9]+(\.[0-9]+)?\s*$";
        /// <summary>cleanStringRegEx is a string representing RegEx
        /// used to clean valid input string in fabric method <see cref="CreateFromString"/></summary>
        private static readonly string cleanStringRegEx = @"([1-9]+[0-9]*(\.[0-9]*[1-9]+)?|0\.[0-9]*[1-9]+)";
        /// <summary>The _fracPrecision local field represents 
        /// a number of fractional digits counted while division for BigFloat instances.</summary>
        private static volatile int _fracPrecision = 20;

        /// <summary>The _dotPos local field represents a delimiter position 
        /// of BigFloat instance in string format.</summary>
        private volatile int _dotPos = 0;
        /// <summary>The _fracLen local field represents a number of digits 
        /// in the fractional part of BigFloat instance.</summary>
        private volatile int _fracLen = -1;
        #endregion

        #region Mutexes
        ///<summary>Random variable used for thread-safe <see cref="DotPos"/>
        ///property first calculation </summary>
        private readonly Object dotPosMutex = new Object();
        ///<summary>Random variable used for thread-safe <see cref="Fractional"/>
        ///property first calculation </summary>
        private readonly Object fracLenMutex = new Object();
        #endregion

        #region Properties
        /// <summary>The DotPos property represents a delimiter position of BigFloat instance in string format.</summary>
        /// <value>The DotPos property gets/private sets the value of the int field, <see cref="_dotPos"/></value>
        public int DotPos
        {
            get
            {
                if (_dotPos == 0)
                {
                    lock (dotPosMutex)
                    {
                        FindDotPos();
                    }
                }
                return _dotPos;
            }
            private set
            {
                _dotPos = value;
            }
        }
        /// <summary>The Integer property represents a number of digits 
        /// in the integer part of BigFloat instance.</summary>
        /// <value>The Integer property gets the value of the int property, <see cref="DotPos"/></value>
        public int Integer
        {
            get
            {
                return DotPos;
            }
        }
        /// <summary>The Fractional property represents a number of digits 
        /// in the fractional part of BigFloat instance.</summary>
        /// <value>The Fractional property gets/private sets the value of the int property, <see cref="_fracLen"/></value>
        public int Fractional
        {
            get
            {
                if (_fracLen < 0)
                    lock (fracLenMutex)
                    {
                        _fracLen = CleanString.Length - DotPos;
                        if (_fracLen > 0)
                            _fracLen--;
                    }
                return _fracLen;
            }
            private set
            {
                _fracLen = value;
            }
        }
        /// <summary>The FracPrecision property represents 
        /// a number of fractional digits counted while division for BigFloat instances.</summary>
        /// <value>The FracPrecision property gets/sets the value of the int property, <see cref="_fracPrecision"/></value>
        public static int FracPrecision
        {
            get
            {
                return _fracPrecision;
            }
            set
            {
                if (value < 0)
                    _fracPrecision = 0;
                else
                    _fracPrecision = value;
            }
        }
        #endregion
    }
}
