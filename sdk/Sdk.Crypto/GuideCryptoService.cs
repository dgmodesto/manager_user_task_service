using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Sdk.Crypto
{
    public class PersonalCryptoService
        : IPersonalCryptoService
    {
        string IPersonalCryptoService.Decrypt(string key, string value)
        {
            ThrowIfNotValid(key, value);

            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(value);

            using var aes = Aes.Create();

            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = iv;
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using MemoryStream memoryStream = new MemoryStream(buffer);
            using CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read);
            using StreamReader streamReader = new StreamReader((Stream)cryptoStream);

            return streamReader.ReadToEnd();
        }

        string IPersonalCryptoService.Encrypt(string key, string value)
        {
            ThrowIfNotValid(key, value);

            byte[] iv = new byte[16];
            byte[] array;

            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using MemoryStream memoryStream = new MemoryStream();
                using CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
                using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                {
                    streamWriter.Write(value);
                }

                array = memoryStream.ToArray();
            }

            return Convert.ToBase64String(array);
        }

        private static void ThrowIfNotValid(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("The value must be filled", nameof(value));
            }

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("The key must be filled", nameof(key));
            }

            if (key.Length != 32)
            {
                throw new ArgumentException("The key must have 32 characters", nameof(key));
            }
        }
    }
}
