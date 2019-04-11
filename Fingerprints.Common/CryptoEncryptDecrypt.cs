using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Fingerprints.Common
{
    public class CryptoEncryptDecrypt
    {
        //public static string EncryptData(string data2Encrypt)
        //{

        //    byte[] plainBytes = null;
        //    try
        //    {
        //        plainBytes = System.Text.Encoding.UTF8.GetBytes(data2Encrypt);
        //        Array.Reverse(plainBytes, 0, plainBytes.Length);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //    }
        //    return EncodeNonAsciiCharacters(Convert.ToBase64String(plainBytes));


        //}
        //public static string DecryptData(string data2Decrypt)
        //{
        //    byte[] getPassword;

        //    try
        //    {
        //        getPassword = Convert.FromBase64String(DecodeEncodedNonAsciiCharacters(data2Decrypt));
        //        Array.Reverse(getPassword, 0, getPassword.Length);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //    }
        //    return System.Text.Encoding.UTF8.GetString(getPassword);
        //}
        //static string EncodeNonAsciiCharacters(string value)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    foreach (char c in value)
        //    {              
        //        string encodedValue = ((int)c).ToString("x4");
        //        sb.Append(encodedValue);
        //    }
        //    return sb.ToString();
        //}
        //static string DecodeEncodedNonAsciiCharacters(string value)
        //{
        //    return Regex.Replace(
        //        value,
        //        @"(?<Value>[a-zA-Z0-9]{4})",
        //        m =>
        //        {
        //            return ((char)int.Parse(m.Groups["Value"].Value, NumberStyles.HexNumber)).ToString();
        //        });
        //}
        public static string Encrypt64(string clearText)
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(clearText));
        }
        public static string Decrypt64(string clearText)
        {
            return Encoding.ASCII.GetString(Convert.FromBase64String(clearText));
        }


        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "FingerPrintsabc123";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "FingerPrintsabc123";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

    }
}
