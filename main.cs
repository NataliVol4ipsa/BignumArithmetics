using System;

namespace net.NataliVol4ica.BignumArithmetics
{
    public static class BignumArithmetics
    {
        public static int Main(string[] args)
        {
            /*try
            {
                FixedPointNumber C;
                
                C = (FixedPointNumber)null - (FixedPointNumber)null;
                Console.WriteLine(C);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception caught: {0}", e.Message);
            }*/
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