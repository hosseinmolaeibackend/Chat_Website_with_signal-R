using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Utilities
{
    public static class Encode
    {
        public static string Encrypte(this string text)
        {
            Byte[] orginalByte;
            Byte[] encrypte;
            MD5 md5;
            md5=new MD5CryptoServiceProvider();
            orginalByte=ASCIIEncoding.Default.GetBytes(text);
            encrypte=md5.ComputeHash(orginalByte);

            return BitConverter.ToString(encrypte);
        }
    }
}
