using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using NBitcoin.DataEncoders;

namespace BsvSimpleLibrary
{
    public class AES_class
    {
        static Aes GetAes(string privateKeyStr)
        {
            ASCIIEncoder asciiEncoder = new ASCIIEncoder();
            byte[] privateKey = asciiEncoder.DecodeData(privateKeyStr);
            // Create a new instance of the Aes class.
            Aes myAes = Aes.Create();
            try
            {
                //This generates a new key and initialization vector (IV).                
                SHA256 sha256 = SHA256.Create();
                byte[] aesKey = sha256.ComputeHash(privateKey, 0, 16);
                myAes.Key = aesKey;
                byte[] aesIV = new byte[16];
                Array.Copy(sha256.ComputeHash(privateKey, 16, 16), aesIV, 16);
                myAes.IV = aesIV;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }
            return (myAes);
        }
        public static byte[] aesEncryptStringToBytes(string plainText, string privateKeyStr)
        {
            byte[] encrypted;
            using (Aes aesAlg = AES_class.GetAes(privateKeyStr))// Aes.Create())
            {
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return encrypted;
        }
        public static string aesDecryptStringFromBytes(byte[] cipherText, string privateKeyStr)
        {
            string plaintext = null;
            using (Aes aesAlg = AES_class.GetAes(privateKeyStr))
            {
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }
    }
}
