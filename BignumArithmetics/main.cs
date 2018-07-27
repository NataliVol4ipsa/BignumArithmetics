using System;
using BignumArithmetics.Parsers;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;

//todo: test parser
//todo: finish BigBased
//todo: add BigFractional
//todo: add more parsers
//todo: append docs

namespace BignumArithmetics
{
    public static class BignumArithmetics
    {
        public static int Main(string[] args)
        {
            try
            {
                Console.WriteLine(2 + 2 * 3 % 2);
                var parser = new BigIntegerRPNParser().Parse("abs(-(2 + 2 * 3 % 2))");
                Console.WriteLine(parser);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.Read();
            return 0;
        } 
    }
}
