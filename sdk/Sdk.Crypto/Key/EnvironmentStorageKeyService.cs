using System;

namespace Sdk.Crypto.Service.Key
{
    public class EnvironmentStorageKeyService : IStorageKeyService
    {
        public virtual bool Save(string key, string keyIdentifier = "server_key.xml")
        {
            var value = Environment.GetEnvironmentVariable(keyIdentifier);

            if (!string.IsNullOrEmpty(value)) return false;

            Environment.SetEnvironmentVariable(keyIdentifier, key);
            return true;

        }

        public virtual string ReadKey(string keyIdentifier)
        {
            return Environment.GetEnvironmentVariable(keyIdentifier);
        }
    }
}
