using Animle.classes;
using Microsoft.Extensions.Options;
using NHibernate.Cfg;
using System;
using System.Security.Cryptography;
using System.Text;

public class EncryptionHelper
{
    private readonly ConfigSettings _appSettings;

    public EncryptionHelper(IOptions<ConfigSettings> options)
    {
        _appSettings = options.Value;
    }

    public byte[] Encrypt(byte[] plainBytes)
    {
        string secret = _appSettings.HashingSercret;
        byte[] key = Encoding.UTF8.GetBytes(secret);
        byte[] encryptedBytes;

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
