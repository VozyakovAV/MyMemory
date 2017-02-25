using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
    public class Crypto
    {
        public static string MD5Hash(string text)
        {
            var password = Encoding.UTF8.GetBytes(text.Trim());

            using (var hmacMD5 = new HMACMD5())
            {
                var saltedHash = hmacMD5.ComputeHash(password);
                return Encoding.UTF8.GetString(saltedHash);
            }
        }
    }
}
