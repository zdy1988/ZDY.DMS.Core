using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ZDY.DMS.Tools
{
    public static class MD5Helper
    {
        public static string GetMD5(string inputValue)
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.UTF8.GetBytes(inputValue));
                var strResult = BitConverter.ToString(result);
                return strResult.Replace("-", "");
            }
        }
    }
}
