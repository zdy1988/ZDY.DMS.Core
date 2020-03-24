using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.StringEncryption
{
    public class NoStringEncryption : IStringEncryption
    {
        public string Decrypt(string input)
        {
            return input;
        }

        public string Encrypt(string input)
        {
            return input;
        }
    }
}
