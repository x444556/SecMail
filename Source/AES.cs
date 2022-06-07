using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SecMail
{
    public static class AES
    {
        public static Tuple<byte[], byte[]> GenerateNewKeyIvPair()
        {
            using (AesManaged aes = new AesManaged())
            {
                aes.KeySize = 256;
                aes.GenerateKey();
                aes.GenerateIV();
                return new Tuple<byte[], byte[]>(aes.Key, aes.IV);
            }
        }
        public static byte[] Encrypt(byte[] plainText, byte[] Key, byte[] IV)
        {
            byte[] encrypted;
            // Create a new AesManaged.    
            using (AesManaged aes = new AesManaged())
            {
                aes.Padding = PaddingMode.Zeros;
                // Create encryptor    
                ICryptoTransform encryptor = aes.CreateEncryptor(Key, IV);
                // Create MemoryStream    
                using (MemoryStream ms = new MemoryStream())
                {
                    // Create crypto stream using the CryptoStream class. This class is the key to encryption    
                    // and encrypts and decrypts data from any given stream. In this case, we will pass a memory stream    
                    // to encrypt    
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(plainText, 0, plainText.Length);
                        for (int i = plainText.Length; i % (128 / 8) != 0; i++) cs.WriteByte(0x00);
                        cs.FlushFinalBlock();
                        encrypted = ms.ToArray();
                    }
                }
            }
            // Return encrypted data    
            return encrypted;
        }
        public static byte[] Decrypt(byte[] cipherText, byte[] Key, byte[] IV)
        {
            byte[] plaintext = new byte[1024 * 1024 * 16];
            // Create AesManaged    
            using (AesManaged aes = new AesManaged())
            {
                aes.Padding = PaddingMode.Zeros;
                // Create a decryptor    
                ICryptoTransform decryptor = aes.CreateDecryptor(Key, IV);
                // Create the streams used for decryption.    
                using (MemoryStream ms = new MemoryStream(cipherText))
                {
                    // Create crypto stream    
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        // Read crypto stream
                        int rbc = cs.Read(plaintext, 0, plaintext.Length);
                        plaintext = plaintext.Take(rbc).ToArray();
                    }
                }
            }
            return plaintext;
        }
    }
}
