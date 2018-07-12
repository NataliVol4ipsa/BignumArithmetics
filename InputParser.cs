using System;
using System.IO;
using System.Collections.Generic;

namespace net.NataliVol4ica.BignumArithmetics
{
    class InputParser : IDisposable
    {
        public InputParser(out List<BigFloat> Numbers, string FileName = null)
        {
            this.FileName = FileName;
            this.Numbers = new List<BigFloat>();
            Numbers = this.Numbers;
        }

        public void ReadInput()
        {
            if (FileName == null)
            {
                this.Numbers.Add(BigFloat.CreateFromString(Console.ReadLine()));
                this.Numbers.Add(BigFloat.CreateFromString(Console.ReadLine()));
            }
            else
            {
                SR = new StreamReader(FileName);
                this.Numbers.Add(BigFloat.CreateFromString(SR.ReadLine()));
                this.Numbers.Add(BigFloat.CreateFromString(SR.ReadLine()));
            }
        }

        public void Dispose()
        {
            if (SR != null)
                SR.Close();
        }

        private string FileName;
        private StreamReader SR;
        private List<BigFloat> Numbers;
    }
}
