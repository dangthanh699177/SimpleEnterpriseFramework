using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEP.Authentication.Security
{
    public class Decrypt
    {
        private IDecrypt strategy;
        public Decrypt()
        {
            this.strategy = null;
        }
        public void SetDecrypt(IDecrypt d)
        {
            this.strategy = d;
        }
        public string Decode(string encodedData)
        {
            return this.strategy.Decode(encodedData);
        }
    }
}
