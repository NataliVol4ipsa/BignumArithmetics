using System;
using System.Collections.Generic;

//totest: 29! = 8841761993739701954543616000000
namespace BignumArithmetics
{
    public static class BignumArithmetics
    {
        /*
        static int errors = 0;
        static void DoTesting(string left, string right, string sum, string dif)
        {
            BigFloat A = BigFloat.CreateFromString(left);
            BigFloat B = BigFloat.CreateFromString(right);

            BigFloat C = A + B;
            BigFloat D = A - B;
            //Console.WriteLine("   {0}\n + {1}", left, right);
            //Console.WriteLine(C);
            //Console.WriteLine(sum);
            //Console.WriteLine("   {0}\n - {1}", left, right);
            //Console.WriteLine(D);
            //Console.WriteLine(dif);
            if (string.Compare(C.ToString(), sum) != 0)
                errors++;
            if (string.Compare(D.ToString(), dif) != 0)
                errors++;
        }
        static void Test()
        {
            Random rnd = new Random();
            int a = rnd.Next(0, Int32.MaxValue);
            int b = rnd.Next(0, Int32.MaxValue);
            a -= Int32.MaxValue / 2;
            b -= Int32.MaxValue / 2;
            decimal A = a;
            decimal B = b;
            A /= 10000;
            B /= 100000;
            decimal C = A + B;
            decimal D = A - B;

            DoTesting(A.ToString().Replace(",", "."),
                B.ToString().Replace(",", "."),
                C.ToString().Replace(",", "."),
                D.ToString().Replace(",", "."));
        }*/

        static void Test()
        {
            BigFloat A = BigFloat.CreateFromNumber(12.4f);
            BigFloat B = BigFloat.CreateFromNumber(0.25f);
            BigFloat C = A * B;
            Console.WriteLine(A);
            Console.WriteLine(B);
            Console.WriteLine(C);
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
            //Console.WriteLine("Fails : " + errors);
            Console.Read();
            return 0;
        }
    }
}
