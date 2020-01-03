using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEP.Authentication.Security
{
    public class EncryptContext
    {
        private IEncrypt strategy;
        public EncryptContext()
        {
            this.strategy = null;
        }
        public void SetEncrypt(IEncrypt e)
        {
            this.strategy = e;
        }
        public string Encode(string plainText)
        {
            return this.strategy.Encode(plainText);
        }
    }
}
