using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace AxGrid.Utils
{
    public static class TextUtils
    {
        public static int[] TextToLong(string input)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                var hashBytes = md5.ComputeHash(inputBytes);
                return new int[]
                {
                    Math.Abs(BitConverter.ToInt32(hashBytes, 0)),
                    Math.Abs(BitConverter.ToInt32(hashBytes, 4)),
                    Math.Abs(BitConverter.ToInt32(hashBytes, 8)),
                    Math.Abs(BitConverter.ToInt32(hashBytes, 12))
                };
            }
        }
        
        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            var sb = new StringBuilder();
            using (var md5 = MD5.Create())
            {
                var inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                var hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                foreach (var t in hashBytes)
                {
                    sb.Append(t.ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}