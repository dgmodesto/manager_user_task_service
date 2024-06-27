using Sdk.Crypto.Service.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Sdk.Crypto.Service.Key
{
    public class RSAKeyService
    {
        private static readonly byte[] RSA_OID = { 0x30, 0xD, 0x6, 0x9, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0xD, 0x1, 0x1, 0x1, 0x5, 0x0 }; // Object ID for RSA

        private const byte INTEGER = 0x2;
        private const byte SEQUENCE = 0x30;
        private const byte BIT_STRING = 0x3;
        private const byte OCTET_STRING = 0x4;

        private readonly RSA _rsa;
        private readonly IStorageKeyService _storage;

        public RSAKeyService(RSA rsa, IStorageKeyService storage)
        {
            _rsa = rsa;
            _rsa.KeySize = 2048;
            _storage = storage;
        }

        /// <summary>
        /// Load a key from xml
        /// </summary>
        /// <param name="xml">Path or XML</param>
        public void FromXmlString(string xml)
        {
            if (string.IsNullOrEmpty(xml))
                throw new IOException("invalid path");

            var xmlString = File.Exists(xml) ? _storage.ReadKey(xml) : xml;

            var parameters = new RSAParameters();

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            if (xmlDoc.DocumentElement.Name.Equals("RSAKeyValue"))
            {
                foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "Modulus": parameters.Modulus = Convert.FromBase64String(node.InnerText); break;
                        case "Exponent": parameters.Exponent = Convert.FromBase64String(node.InnerText); break;
                        case "P": parameters.P = Convert.FromBase64String(node.InnerText); break;
                        case "Q": parameters.Q = Convert.FromBase64String(node.InnerText); break;
                        case "DP": parameters.DP = Convert.FromBase64String(node.InnerText); break;
                        case "DQ": parameters.DQ = Convert.FromBase64String(node.InnerText); break;
                        case "InverseQ": parameters.InverseQ = Convert.FromBase64String(node.InnerText); break;
                        case "D": parameters.D = Convert.FromBase64String(node.InnerText); break;
                    }
                }
            }
            else
            {
                throw new Exception("Invalid XML RSA key.");
            }

            _rsa.ImportParameters(parameters);
        }
        public string ToXmlString(bool includePrivateParameters = true)
        {
            var parameters = _rsa.ExportParameters(includePrivateParameters);
            var sb = new StringBuilder();
            sb.Append("<RSAKeyValue>");
            foreach (var item in parameters.ToDictionary(includePrivateParameters))
            {
                sb.AppendFormat("<{0}>{1}</{0}>", item.Key, Convert.ToBase64String(item.Value));
            }
            sb.Append("</RSAKeyValue>");

            return sb.ToString();
        }

        public string ToXmlString(string keyName)
        {
            var key = ToXmlString();
            _storage.Save(key, keyName);
            return key;
        }

        public string PublicKeyToXmlString(string keyName)
        {
            var key = ToXmlString(false);
            _storage.Save(key, keyName);
            return key;
        }

        private string ToPem(bool includePrivateParameters = true)
        {
            var parameters = _rsa.ExportParameters(includePrivateParameters);
            var arrBinaryKey = new List<byte>();
            var description = "RSA PUBLIC";
            foreach (var item in parameters.ToDictionary(includePrivateParameters))
            {
                arrBinaryKey.InsertRange(0, item.Value);
                AppendLength(ref arrBinaryKey, item.Value.Length);
                arrBinaryKey.Insert(0, INTEGER);
            }

            if (includePrivateParameters)
            {
                description = "RSA PRIVATE";
                arrBinaryKey.Insert(0, 0x00);
                AppendLength(ref arrBinaryKey, 1);
                arrBinaryKey.Insert(0, INTEGER);

                AppendLength(ref arrBinaryKey, arrBinaryKey.Count);
                arrBinaryKey.Insert(0, SEQUENCE);
            }
            else
            {
                AppendLength(ref arrBinaryKey, arrBinaryKey.Count);
                arrBinaryKey.Insert(0, SEQUENCE);

                arrBinaryKey.Insert(0, 0x0);
                AppendLength(ref arrBinaryKey, arrBinaryKey.Count);

                arrBinaryKey.Insert(0, BIT_STRING);
                arrBinaryKey.InsertRange(0, RSA_OID);

                AppendLength(ref arrBinaryKey, arrBinaryKey.Count);
                arrBinaryKey.Insert(0, SEQUENCE);
            }

            return FormatPemString(System.Convert.ToBase64String(arrBinaryKey.ToArray()), description);
        }

        public string ToPem(string keyName)
        {
            var key = ToPem();
            _storage.Save(key, keyName);
            return key;
        }

        public string PublicKeyToPem(string keyName)
        {
            var key = ToPem(false);
            _storage.Save(key, keyName);
            return key;
        }

        private static string FormatPemString(string key, string description)
        {
            var sb = new StringBuilder(key);
            ;
            for (var loop = 64; loop < key.Length; loop += 65)
            {
                sb.Insert(loop, "\n");
            }

            return $"-----BEGIN {description} KEY-----\n{sb.ToString()}\n-----END {description} KEY-----";
        }

        private static void AppendLength(ref List<byte> arrBinaryData, int nLen)
        {
            if (nLen <= byte.MaxValue)
            {
                arrBinaryData.Insert(0, Convert.ToByte(nLen));
                arrBinaryData.Insert(0, 0x81);
            }
            else
            {
                arrBinaryData.Insert(0, Convert.ToByte(nLen % (byte.MaxValue + 1)));
                arrBinaryData.Insert(0, Convert.ToByte(nLen / (byte.MaxValue + 1)));
                arrBinaryData.Insert(0, 0x82);
            }

        }
    }
}
