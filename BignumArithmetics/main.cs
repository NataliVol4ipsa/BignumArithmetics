using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace BignumArithmetics
{
    public static class BignumArithmetics
    {
        static Random rnd = new Random((int)DateTime.Now.Ticks);
        static int errors = 0;
        static void DoTesting(string left, string right, string sum, string dif, string mul)
        {
            BigFloat A = BigFloat.CreateFromString(left);
            BigFloat B = BigFloat.CreateFromString(right);

            BigFloat C = A + B;
            BigFloat D = A - B;
            BigFloat E = A * B;            
           
            if (string.Compare(C.ToString(), sum) != 0)
            {
                Console.WriteLine("   {0}\n + {1}", left, right);
                Console.WriteLine("My   {0}", C);
                Console.WriteLine("Orig {0}", sum);
                errors++;
            }
            if (string.Compare(D.ToString(), dif) != 0)
            {
                Console.WriteLine("   {0}\n - {1}", left, right);
                Console.WriteLine("My   {0}", D);
                Console.WriteLine("Orig {0}", dif);
                errors++;
            }
            if (string.Compare(E.ToString(), mul) != 0)
            {
                Console.WriteLine("   {0}\n * {1}", left, right);
                Console.WriteLine("My   {0}", E);
                Console.WriteLine("Orig {0}", mul);
                errors++;
            }
        }

        static string DecimalToString(decimal number)
        {
            string str = number.ToString();
            if (!str.Contains(","))
                return str;
            str = str.Replace(",", ".");
            StringBuilder sb = new StringBuilder(str);
            while (sb[sb.Length - 1] == '0')
                sb.Remove(sb.Length - 1, 1);
            if (sb[sb.Length - 1] == '.')
                sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        static void Test()
        {
            int a = rnd.Next(0, Int32.MaxValue);
            int b = rnd.Next(0, Int32.MaxValue);
            a -= Int32.MaxValue / 2;
            b -= Int32.MaxValue / 2;
            decimal A = a;
            decimal B = b;
            A /= 10000;
            B /= 100000;
            decimal C = A + B;
            decimal D = A - B;
            decimal E = A * B;

            DoTesting(DecimalToString(A),
                    DecimalToString(B),
                    DecimalToString(C),
                    DecimalToString(D),
                    DecimalToString(E));
        }
        static void DoDivTesting(string left, string right, string div)
        {
            BigFloat A = BigFloat.CreateFromString(left);
            BigFloat B = BigFloat.CreateFromString(right);

            BigFloat C = A / B;

            if (string.Compare(C.ToString(), div) != 0)
            {
                Console.WriteLine("   {0}\n / {1}", left, right);
                Console.WriteLine("My   {0}", C);
                Console.WriteLine("Orig {0}", div);
                errors++;
            }
        }
        static void Test1()
        {
            int a = rnd.Next(0, Int32.MaxValue);
            int b = rnd.Next(0, Int32.MaxValue);
            a -= Int32.MaxValue / 2;
            b -= Int32.MaxValue / 2;
            decimal A = a;
            decimal B = b;
            A /= 10000;
            B /= 100000;
            decimal C = A * B;


            DoDivTesting(DecimalToString(C),
                    DecimalToString(B),
                    DecimalToString(A));
        }
        static void ConcreteTest1()
        {
            int a = rnd.Next(0, Int32.MaxValue);
            int b = rnd.Next(0, Int32.MaxValue);
            a -= Int32.MaxValue / 2;
            b -= Int32.MaxValue / 2;
            decimal C = 1849752.341710464m;
            decimal B = -21.10336m;
            decimal A = C / B;
            
            DoDivTesting(DecimalToString(C),
                    DecimalToString(B),
                    DecimalToString(A));
        }
        public static int Main(string[] args)
        {
            int i = 0;
            try
            {
                for (i = 0; i < 10000; i++)
                Test1();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine("Fails : {0} | {1}", errors, i);
            Console.Read();
            return 0;
        }
    }
}
