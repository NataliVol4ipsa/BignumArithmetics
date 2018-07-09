using System;
using System.Collections.Generic;

namespace net.NataliVol4ica.BignumArithmetics
{
    static class BigSum
    {
        public static FixedPointNumber Count(FixedPointNumber A, FixedPointNumber B)
        {
            List<int> sum = new List<int>();
            int i = 0;
            int j = 0;

            int maxFrac;
            int indexDif;
            int plus;

            if (A.GetFracLen() < B.GetFracLen())
                FixedPointNumber.Swap(ref A, ref B);
            maxFrac = A.GetFracLen();
            indexDif = A.GetFracLen() - B.GetFracLen();
            plus = 0;
            
            while (i < indexDif)
            {
                sum.Add(A[i]);
                i++;
            }
            while (i < maxFrac)
            {
                sum.Add(A[i] + B[j] + plus);
                plus = sum[i] / 10;
                sum[i] %= 10;
                i++;
                j++;
            }
            /* eo dot */
            if (A.GetIntLen() < B.GetIntLen())
            {
                FixedPointNumber.Swap(ref A, ref B);
                FixedPointNumber.Swap(ref i, ref j);
            }
            while (j < B.Size)
            {
                sum.Add(A[i] + B[j] + plus);
                plus = sum[sum.Count - 1] / 10;
                sum[sum.Count - 1] %= 10;
                i++;
                j++;
            }
            while (i < A.Size)
            {
                sum.Add(A[i] + plus);
                plus = sum[sum.Count - 1] / 10;
                sum[sum.Count - 1] %= 10;
                i++;
            }
            while (plus > 0)
            {
                sum.Add(plus % 10);
                plus /= 10;
            }
            sum.Reverse();
            return (new FixedPointNumber(sum, sum.Count - maxFrac));
        }
    }
}
