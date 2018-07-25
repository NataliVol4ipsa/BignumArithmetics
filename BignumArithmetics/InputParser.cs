using System;
using System.IO;
using System.Collections.Generic;

namespace BignumArithmetics
{
    class InputParser : IDisposable
    {
        public InputParser(out List<BigDecimal> Numbers, string FileName = null)
        {
            this.FileName = FileName;
            this.Numbers = new List<BigDecimal>();
            Numbers = this.Numbers;
        }

        public void ReadInput()
        {
            if (FileName == null)
            {
                this.Numbers.Add(BigDecimal.CreateFromString(Console.ReadLine()));
                this.Numbers.Add(BigDecimal.CreateFromString(Console.ReadLine()));
            }
            else
            {
                SR = new StreamReader(FileName);
                this.Numbers.Add(BigDecimal.CreateFromString(SR.ReadLine()));
                this.Numbers.Add(BigDecimal.CreateFromString(SR.ReadLine()));
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
