using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class EncryptionHelper
{
    // Convert hex string to byte array

    public static byte[] Encrypt(byte[] plainBytes)
    {
         var configuration = new ConfigurationBuilder()
         .AddJsonFile("appsettings.json")
          .Build();

        string secret = configuration.GetSection("AppSettings:HashingSercret").Value;
        byte[] key = Encoding.UTF8.GetBytes(secret);
        byte[] encryptedBytes = null;

        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;

            using (ICryptoTransform encryptor = aes.CreateEncryptor())
            {
                encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
            }
        }

        return encryptedBytes;
    }
}
