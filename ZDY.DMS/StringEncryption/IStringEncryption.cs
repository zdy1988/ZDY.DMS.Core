using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.StringEncryption
{
    public interface IStringEncryption
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        string Encrypt(string input);

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        string Decrypt(string input);
    }
}
