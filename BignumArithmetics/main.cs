using System;
using BignumArithmetics.Parsers;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;

//todo: class structure https://referencesource.microsoft.com/#mscorlib/system/collections/generic/list.cs,cf7f4095e4de7646
//todo: write docs for parser

namespace BignumArithmetics
{
    public static class BignumArithmetics
    {
        public static int Main(string[] args)
        {
            try
            {
                Console.WriteLine(2 + 2 * 3 % 2);
                var parser = new BigIntegerRPNParser("abs(-(2 + 2 * 3 % 2))").Parse();
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
