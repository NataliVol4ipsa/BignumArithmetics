using System;
using System.Collections.Generic;

//totest: 29! = 8841761993739701954543616000000
namespace net.NataliVol4ica.BignumArithmetics
{
    public static class BignumArithmetics
    {
        static void Test()
        {
            BigFloat actual = BigFloat.CreateFromString("-0.156");

            Console.WriteLine("Int {0}, Frac {1}", actual.Integer, actual.Fracial);
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
