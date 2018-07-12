using System;
using System.Collections.Generic;

//totest: 29! = 8841761993739701954543616000000
namespace BignumArithmetics
{
    public static class BignumArithmetics
    {
        static void DoTesting(string left, string right, string sum, string dif)
        {
            BigFloat A = BigFloat.CreateFromString(left);
            BigFloat B = BigFloat.CreateFromString(right);

            //BigFloat C = A + B;
            BigFloat D = A - B;
            //Console.WriteLine(C);
            //Console.WriteLine(sum);
            Console.WriteLine(D);
            Console.WriteLine(dif);
        }
        static void Test()
        {
            DoTesting("123.45", "45.678", "169.128", "77.772");
        }

        public static int Main(string[] args)
        {
            try
            {
                Test();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.Read();
            return 0;
        }
    }
}
