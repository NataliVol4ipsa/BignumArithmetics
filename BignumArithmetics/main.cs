using System;
using BignumArithmetics.Parsers;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;

//todo: class structure https://referencesource.microsoft.com/#mscorlib/system/collections/generic/list.cs,cf7f4095e4de7646

namespace BignumArithmetics
{
    public static class BignumArithmetics
    {
        public static int Main(string[] args)
        {
            try
            {
                var parser = new BigIntegerRPNParser("2 + 2").Parse();
                Console.WriteLine(parser);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            /*int lastMatchPos = 0;
            int lastMatchLen = 0;
            string test = "   (2 + 2 )* 2    ";
            test = test.Trim();
            string reg1 = @"\G\s*({0}|\+|-|\*|\\|%|\(|\))\s*";
            string reg = String.Format(reg1, @"\d+"); //insert exact regex
            Match match = Regex.Match(test, reg);
            while (match.Success)
            {
                lastMatchPos = match.Index;
                lastMatchLen = match.Value.Length;
                Console.WriteLine(match.Value);
                match = match.NextMatch();
            }
            Console.WriteLine("Last match {0} of len {1} at string of len {2}", lastMatchPos, lastMatchLen, test.Length);
            if (lastMatchPos + lastMatchLen < test.Length)
                Console.WriteLine("Incorrect string");*/

            Console.Read();
            return 0;
        } 
    }
}
