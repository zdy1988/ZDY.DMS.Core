using System;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace ZDY.DMS.AspNetCore.Auth
{
    public class SecurityTokenOptions
    {
        public static string Audience { get; } = "Audience";
        public static string Issuer { get; } = "ZDYLA.COM";
        public static RsaSecurityKey Key { get; } = new RsaSecurityKey(GenerateKey());
        public static SigningCredentials SigningCredentials { get; } = new SigningCredentials(Key, SecurityAlgorithms.RsaSha256Signature);
        public static TimeSpan ExpiresSpan { get; } = TimeSpan.FromMinutes(60 * 10);

        private static RSAParameters GenerateKey()
        {
            using (var key = new RSACryptoServiceProvider(2048))
            {
                return key.ExportParameters(true);
            }
        }
    }
}
