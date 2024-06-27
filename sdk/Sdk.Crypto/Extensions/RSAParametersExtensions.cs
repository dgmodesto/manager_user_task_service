using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Xml;

namespace Sdk.Crypto.Service.Extensions
{
    public static class RSAParametersExtensions
    {
        public static IDictionary<string, byte[]> ToDictionary(this RSAParameters param, bool includePrivateKey = true)
        {
            var response = new Dictionary<string, byte[]>();
            if (includePrivateKey)
            {
                response.Add("InverseQ", param.InverseQ);
                response.Add("DQ", param.DQ);
                response.Add("DP", param.DP);
                response.Add("Q", param.Q);
                response.Add("P", param.P);
                response.Add("D", param.D);
            }
            response.Add("Exponent", param.Exponent);
            response.Add("Modulus", param.Modulus);
            return response;
        }


        public static RSAParameters FromXmlString(this string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new IOException("invalid key");
            }

            var parameters = new RSAParameters();

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(key);

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

            return parameters;
        }
    }
}
