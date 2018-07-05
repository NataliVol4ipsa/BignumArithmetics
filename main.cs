using System;
using System.Collections.Generic;

namespace net.NataliVol4ica.BignumArithmetics
{
    public static class BignumArithmetics
    {
        public static int Main(string[] args)
        {
            /* Reading */
            List<FixedPointNumber> numbers;
            try
            {
                using (InputParser ip = new InputParser(out numbers, "Input.txt"))
                {
                    ip.ReadInput();
                }
                Console.WriteLine("1: {0}.{1}", numbers[0].GetIntLen(), numbers[0].GetFracLen());
                Console.WriteLine("2: {0}.{1}", numbers[1].GetIntLen(), numbers[1].GetFracLen());
                FixedPointNumber C = numbers[0] + numbers[1];
                Console.WriteLine(C);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception caught: {0}", e.Message);
            }
            //Console.WriteLine((new FixedPointNumber ()).GetType());
            Console.Read();
            return 0;
        }
    }
}

//readonly????
//create static class with swappers, converters?

//thoughts for abstract class
//if gettype(a) != gettype(b) exception
//else cast to relevant class object and call operator