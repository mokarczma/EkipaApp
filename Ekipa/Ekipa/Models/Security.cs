using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Ekipa.Models
{ 
    public class Security
    {
        public static string sha512encrypt(string password)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            SHA512Managed sha512hasher = new SHA512Managed();
            byte[] hashedDataBytes = sha512hasher.ComputeHash(encoder.GetBytes(password));
            return byteArrayToString(hashedDataBytes);

        }
        public static string byteArrayToString(byte[] inputArray)
        {
            StringBuilder sr = new StringBuilder("");
            for (int i = 0; i < inputArray.Length; i++)
            {
                sr.Append(inputArray[i].ToString("X2"));
            }
            return sr.ToString();
        }
    }
}