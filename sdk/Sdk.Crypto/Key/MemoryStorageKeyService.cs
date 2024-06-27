using System;
using System.Collections.Generic;

namespace Sdk.Crypto.Service.Key
{
    public class MemoryStorageKeyService : IStorageKeyService
    {
        private readonly Dictionary<string, string> _dictionary;

        public MemoryStorageKeyService()
        {
            _dictionary = new Dictionary<string, string>();
        }

        public string ReadKey(string keyIdentifier)
        {
            if (_dictionary.ContainsKey(keyIdentifier))
            {
                return _dictionary[keyIdentifier];
            }
            throw new Exception("Key not found");
        }

        public bool Save(string xml, string keyIdentifier)
        {
            if (_dictionary.ContainsKey(keyIdentifier))
            {
                throw new Exception("KeyIdentityer already exists");
            }
            _dictionary.Add(keyIdentifier, xml);
            return true;
        }
    }
}
