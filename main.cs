using System;
using System.Collections.Generic;

//totest: 29! = 8841761993739701954543616000000
namespace BignumArithmetics
{
    public static class BignumArithmetics
    {
        static void Test()
        {
            var A = BigFloat.CreateFromString("+123.45");
            var B = BigFloat.CreateFromString("+00045.678");

            BigFloat C = A + B;
            Console.WriteLine(C);
            Console.WriteLine(123.45 + 45.678);

            Console.WriteLine();

            A = BigFloat.CreateFromString("+123.45");
            B = BigFloat.CreateFromString("+00045.678");

            C = A - B;
            Console.WriteLine(C);
            Console.WriteLine(123.45 - 45.678);
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
