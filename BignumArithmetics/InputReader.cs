using System;
using System.IO;
using System.Collections.Generic;

namespace BignumArithmetics
{
    public class InputReader : IDisposable
    {
        public InputReader(out List<BigDecimal> Numbers, string FileName = null)
        {
            this.FileName = FileName;
            this.Numbers = new List<BigDecimal>();
            Numbers = this.Numbers;
        }

        public void ReadInput()
        {
            if (FileName == null)
            {
                this.Numbers.Add(new BigDecimal(Console.ReadLine()));
                this.Numbers.Add(new BigDecimal(Console.ReadLine()));
            }
            else
            {
                SR = new StreamReader(FileName);
                this.Numbers.Add(new BigDecimal(SR.ReadLine()));
                this.Numbers.Add(new BigDecimal(SR.ReadLine()));
            }
        }

        public void Dispose()
        {
            if (SR != null)
                SR.Close();
        }

        private string FileName;
        private StreamReader SR;
        private List<BigDecimal> Numbers;
    }
}
