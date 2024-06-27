using Sdk.Crypto.Service.Extensions;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Sdk.Crypto.Service
{
    public class CryptoService
    {
        private readonly RSA _rsa;
        public CryptoService(string key)
        {
            var rsaParameters = key.FromXmlString();
            _rsa = RSA.Create(rsaParameters);
        }

        public string Encrypt(string value)
        {
            const int blockSize = 245;
            var encryptedBytes = 0;
            var sb = new StringBuilder();
            while (encryptedBytes < value.Length)
            {
                var block = GetBlock(value, blockSize, encryptedBytes);
                var line = Convert.ToBase64String(_rsa.Encrypt(Encoding.ASCII.GetBytes(block), RSAEncryptionPadding.Pkcs1));
                sb.Append(line);
                encryptedBytes += blockSize;
            }
            return sb.ToString();
        }


        public string Decrypt(string value)
        {
            var encryptedBytes = 0;
            var response = new StringBuilder();
            const int blockSize = 344;
            while (encryptedBytes < value.Length)
            {
                var bytesData = Convert.FromBase64String(GetBlock(value, blockSize, encryptedBytes));
                response.Append(Encoding.ASCII.GetString(_rsa.Decrypt(bytesData, RSAEncryptionPadding.Pkcs1)));
                encryptedBytes += blockSize;
            }
            return response.ToString();
        }

        private static string GetBlock(string value, int blockSize, int encryptedBytes)
        {
            return blockSize + encryptedBytes >= value.Length ? value.Substring(encryptedBytes)
                                                : value.Substring(encryptedBytes, blockSize);
        }
    }

}
