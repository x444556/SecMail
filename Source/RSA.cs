using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SecMail
{
    public static class RSA
    {
        public const int KeySize = 2048;
        public static RSAParameters NewPrivateKey()
        {
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(KeySize))
            {
                return RSA.ExportParameters(true);
            }
        }
        public static RSAParameters GetPublicKey(RSAParameters privateKey)
        {
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSA.ImportParameters(privateKey);
                return RSA.ExportParameters(false);
            }
        }
        public static byte[] Encrypt(byte[] data, RSAParameters publicKey)
        {
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSA.ImportParameters(publicKey);
                return RSA.Encrypt(data, true);
            }
        }
        public static byte[] Decrypt(byte[] data, RSAParameters privateKey)
        {
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSA.ImportParameters(privateKey);
                return RSA.Decrypt(data, true);
            }
        }
        public static byte[] SignData(byte[] data, RSAParameters privateKey)
        {
            using(RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSA.ImportParameters(privateKey);
                return RSA.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
        }
        public static bool VeriySignature(byte[] signature, byte[] data, RSAParameters publicKey)
        {
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSA.ImportParameters(publicKey);
                return RSA.VerifyData(data, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
        }
    }
}
