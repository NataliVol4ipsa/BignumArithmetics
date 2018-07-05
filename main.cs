using System;
using System.Collections.Generic;

namespace net.NataliVol4ica.BignumArithmetics
{
    public static class BignumArithmetics
    {
        public static int Main(string[] args)
        {
            /* Reading */
            try
            {
                List<FixedPointNumber> numbers;
                using (InputParser ip = new InputParser(out numbers, "Input.txt"))
                {
                    ip.ReadInput();
                }
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
