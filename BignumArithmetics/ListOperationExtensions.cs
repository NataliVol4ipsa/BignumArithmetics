using System;
using System.Collections.Generic;
using System.Linq;

//TODO: TESTS?

namespace BignumArithmetics
{
    static class ListOperationExtensions
    {
        public delegate void Normalizer(List<int> list);

        /// <summary> Generates a list representing sum of two reversed digit lists</summary>
        /// <param name="leftList">First operand</param>
        /// <param name="rightList">Second operand</param>
        /// should be called for result</param>
        /// <returns>List representing sum of two lists in same reversed form</returns>
        public static List<int> SumWithList(this List<int> leftList, List<int> rightList)
        {
            if (leftList.Count <= 0 || rightList.Count <= 0)
                return new List<int>();
            int maxlen = Math.Max(leftList.Count, rightList.Count);
            leftList.AddRange(Enumerable.Repeat(0, maxlen - leftList.Count));
            rightList.AddRange(Enumerable.Repeat(0, maxlen - rightList.Count));
            var resultList = new List<int>(leftList.Count);
            for (int i = 0; i < maxlen; i++)
                resultList.Add(leftList[i] + rightList[i]);
            return resultList;
        }
        /// <summary> Generates a list representing substraction of two reversed digit lists.</summary>
        /// <param name="leftList">First list.
        /// Number represented by leftList has to be bigger than the one by rightList!</param>
        /// <param name="rightList">Second list.</param>
        /// should be called for result</param>
        /// <returns>List representing substraction of two lists in same reversed form</returns>
        /// <exception cref="ArgumentException">Exception thrown if leftList < rightList</exception>
        public static List<int> SubByList(this List<int> leftList, List<int> rightList)
        {
            if (leftList.CompareWithList(rightList) < 0)
                throw new ArgumentException("leftList cannot be bigger than rightList!");
            rightList.AddRange(Enumerable.Repeat(0, leftList.Count - rightList.Count));
            var resultList = new List<int>();
            for (int i = 0; i < leftList.Count; i++)
                resultList.Add(leftList[i] - rightList[i]);
            return resultList;
        }
        /// <summary> Generates a list representing multiplication of two reversed digit lists.</summary>
        /// <param name="leftList">First list.</param>
        /// <param name="rightList">Second list.</param>
        /// should be called for result</param>
        /// <returns>List representing multiplication of two lists in same reversed form</returns>
        public static List<int> MulWithList(this List<int> leftList, List<int> rightList)
        {
            var resultList = new List<int>();
            var tempList = new List<int>();
            resultList = leftList.MulWithDigit(rightList[0]);
            for (int i = 1; i < rightList.Count; i++)
            {
                if (rightList[i] == 0)
                    continue;
                tempList = leftList.MulWithDigit(rightList[i], i);
                resultList = resultList.SumWithList(tempList);
            }
            return resultList;
        }
        /// <summary> Generates a list representing division of two reversed digit lists.</summary>
        /// <param name="leftList">First list.</param>
        /// <param name="rightList">Second list.</param>
        /// <param name="normFunc">Delegate <see cref="Normalizer"/>
        /// of reversed digit list normalization method</param>
        /// <param name="remainder">A list equal to leftList mod rightList"/>
        /// <returns>List representing division of two lists in same reversed form</returns>
        public static List<int> DivByList(this List<int> leftList, List<int> rightList, 
                                            Normalizer NormFunc, out List<int> remainder)
        {
            var resultList = new List<int>();
            remainder = new List<int>();
            if (leftList.CompareWithList(rightList) >= 0)
            {
                bool unnormed = true;
                int sum, dif;
                int indexToAdd = leftList.Count - rightList.Count - 1;
                remainder.AddRange(leftList.GetRange(indexToAdd + 1, rightList.Count));
                do
                {
                    if (unnormed)
                        while (remainder.Count > 0 && remainder.Last() == 0)
                            remainder.RemoveAt(remainder.Count - 1);
                    unnormed = true;
                    sum = 0;
                    while (remainder.CompareWithList(rightList) >= 0)
                    {
                        dif = remainder.Count - rightList.Count;
                        rightList.AddRange(Enumerable.Repeat(0, dif));
                        remainder = remainder.SubByList(rightList);
                        NormFunc(remainder);
                        if (dif > 0)
                            rightList.RemoveAt(rightList.Count - 1);
                        while (remainder.Count > 0 && remainder.Last() == 0)
                            remainder.RemoveAt(remainder.Count - 1);
                        sum++;
                        unnormed = false;
                    }
                    resultList.Add(sum);
                    if (indexToAdd >= 0)
                        remainder.Insert(0, leftList[indexToAdd]);
                    indexToAdd--;
                } while (indexToAdd >= -1);
            }
            if (resultList.Count == 0)
                resultList.Add(0);
            resultList.Reverse();
            return resultList;
        }
        /// <summary> Generates a list representing multiplication of reversed digit lists and digit</summary>
        /// <param name="leftList">A list to be multiplicated</param>
        /// <param name="digit">A digit to be multiplicated</param>
        /// <param name="padding">Optional parameter representing
        /// number of zeroes to be padded at the beginning of result list</param>
        /// <returns>List representing multiplication of reversed digit lists and digit</returns>
        public static List<int> MulWithDigit(this List<int> leftList, int digit, int padding = 0)
        {
            if (digit == 0)
                return new List<int> { 0 };
            var resultList = new List<int>(leftList.Count);
            resultList.AddRange(Enumerable.Repeat(0, padding));
            for (int i = 0; i < leftList.Count; i++)
                resultList.Add(leftList[i] * digit);
            return resultList;
        }

        /// <summary> Compares two reversed list of digits 
        /// and returns an int representing their order when sorted </summary>
        /// <param name="left">First list to compare</param>
        /// <param name="right">Second list to compare</param>
        /// <returns>An int representing list order when sorted</returns>
        public static int CompareWithList(this List<int> left, List<int> right)
        {
            if (left.Count != right.Count)
                return left.Count - right.Count;
            int i = left.Count - 1;
            while (i >= 0 && left[i] == right[i])
                i--;
            if (i == -1)
                return 0;
            return left[i] - right[i];
        }
        /// <summary>Removes extra tailing zeroes from digit list</summary>
        /// <param name="list">List to be cleaned</param>
        public static void RemoveTailingZeros(this List<int> list)
        {
            while (list.Last() == 0 && list.Count > 1)
                list.RemoveAt(list.Count - 1);
        }
    }
}
