using System;
using System.Collections.Generic;

namespace net.NataliVol4ica.BignumArithmetics
{
    public static class BignumArithmetics
    {
        static void Test()
        {
            BigFloat actual = new BigFloat("-0.156");

            Console.WriteLine("Int {0}, Frac {1}", actual.Integer, actual.Fracial);
        }

        public static int Main(string[] args)
        {
            try
            {
                List<int> test = new List<int>();
                test.Add(1);
                test.Add(2);
                test.Add(3);
                test.Add(4);
                test.Add(5);
                Console.WriteLine(test.ToString());
                //Test();
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

//totest 29! = 8841761993739701954543616000000

//readonly????
//create static class with swappers, converters?

//Regex.Replace(RawString, @"\s+", "");  - removes ws-s

//thoughts for abstract class
//if gettype(a) != gettype(b) exception
//else cast to relevant class object and call operator