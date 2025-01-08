using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Portfolio.Domain.HelperClass
{
    public static class StaticHelper
    {
        static string key = GenerateKey();
        public static string EncryptString(string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;
            using (Aes aes = Aes.Create())
            {
                aes.Key=Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform encryptor=aes.CreateEncryptor(aes.Key,aes.IV);
                using (MemoryStream memoryStream=new MemoryStream())
                {
                    using (CryptoStream cryptostream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptostream))
                        {
                            streamWriter.Write(plainText);
                        }
                            array=memoryStream.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(array);
        }

        public static string decryptString(string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer=Convert.FromBase64String(cipherText);
            using (Aes aes = Aes.Create())
            {
                aes.Key=Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor=aes.CreateDecryptor(aes.Key,aes.IV);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptostream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Write))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptostream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        public static string GenerateKey(int size=32)
        {
            byte[] keyBytes=new byte[size];
            using(var randomNo=RandomNumberGenerator.Create())
            {
                randomNo.GetBytes(keyBytes);
            }
            return Convert.ToBase64String(keyBytes);    
        }

    }
}
