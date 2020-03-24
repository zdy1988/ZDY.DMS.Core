using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ZDY.DMS.StringEncryption
{
    public class MD5StringEncryption : IStringEncryption
    {
        public string Decrypt(string input)
        {
            throw new NotImplementedException();
        }

        public string Encrypt(string input)
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                var strResult = BitConverter.ToString(result);
                return strResult.Replace("-", "");
            }
        }
    }
}
