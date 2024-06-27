using Sdk.Crypto.Service.Key;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Sdk.Crypto.Service.Service
{
    public class RSAService
    {
        private readonly IStorageKeyService _storage;

        public RSAService(IStorageKeyService storage)
        {
            _storage = storage;
        }
        public string Encrypt(string pathOrkey, string value)
        {
            using var rsa = RSA.Create();

            var keyService = new RSAKeyService(rsa, _storage);
            const int blockSize = 245;
            keyService.FromXmlString(pathOrkey);
            var encryptedBytes = 0;
            var sb = new StringBuilder();
            while (encryptedBytes < value.Length)
            {
                var block = GetBlock(value, blockSize, encryptedBytes);
                var line = Convert.ToBase64String(rsa.Encrypt(Encoding.ASCII.GetBytes(block), RSAEncryptionPadding.Pkcs1));
                sb.Append(line);
                encryptedBytes += blockSize;
            }

            return sb.ToString();
        }

        private static string GetBlock(string value, int blockSize, int encryptedBytes)
        {
            return blockSize + encryptedBytes >= value.Length ? value.Substring(encryptedBytes)
                                                : value.Substring(encryptedBytes, blockSize);
        }

        public string Decrypt(string pathOrkey, string value)
        {
            using var rsa = RSA.Create();

            var keyService = new RSAKeyService(rsa, _storage);
            keyService.FromXmlString(pathOrkey);
            var encryptedBytes = 0;
            var response = new StringBuilder();
            const int blockSize = 344;
            while (encryptedBytes < value.Length)
            {
                var bytesData = Convert.FromBase64String(GetBlock(value, blockSize, encryptedBytes));
                response.Append(Encoding.ASCII.GetString(rsa.Decrypt(bytesData, RSAEncryptionPadding.Pkcs1)));
                encryptedBytes += blockSize;
            }
            return response.ToString();
        }
    }
}
