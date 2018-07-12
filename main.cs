using System;
using System.Collections.Generic;

//totest: 29! = 8841761993739701954543616000000
namespace net.NataliVol4ica.BignumArithmetics
{
    public static class BignumArithmetics
    {
        static void Test()
        {
            var list = new List<int> { 7, 6, 5 };
            string actual = BigFloat.IntListToString(list, 3);
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
