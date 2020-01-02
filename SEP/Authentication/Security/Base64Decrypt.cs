using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEP.Authentication.Security
{
    public class Base64Decrypt : IDecrypt
    {
        public Base64Decrypt()
        {
        }

        public string Decode(string encodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(encodedData);
            return System.Text.ASCIIEncoding.ASCII.GetString(base64EncodedBytes);
        }
    }
}
