using System;
using System.IO;
using System.Collections.Generic;

namespace net.NataliVol4ica.BignumArithmetics
{
    class InputParser : IDisposable
    {
        public InputParser(out List<FixedPointNumber> Numbers, string FileName = null)
        {
            this.FileName = FileName;
            this.Numbers = new List<FixedPointNumber>();
            Numbers = this.Numbers;
        }

        public void ReadInput()
        {
            if (FileName == null)
            {
                this.Numbers.Add(new FixedPointNumber(Console.ReadLine()));
                this.Numbers.Add(new FixedPointNumber(Console.ReadLine()));
            }
            else
            {
                SR = new StreamReader(FileName);
                this.Numbers.Add(new FixedPointNumber(SR.ReadLine()));
                this.Numbers.Add(new FixedPointNumber(SR.ReadLine()));
            }
        }

        public void Dispose()
        {
            if (SR != null)
                SR.Close();
        }

        private string FileName;
        private StreamReader SR;
        private List<FixedPointNumber> Numbers;
    }
}
