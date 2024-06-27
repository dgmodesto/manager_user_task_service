using System.IO;
using System.Text;

namespace Sdk.Crypto.Service.Key
{
    public class LocalStorageKeyService : IStorageKeyService
    {
        public virtual bool Save(string xml, string keyIdentifier = "server_key.xml")
        {
            var data = Encoding.UTF8.GetBytes(xml);

            File.WriteAllBytes(keyIdentifier, data);

            return true;
        }

        public virtual string ReadKey(string keyIdentifier)
        {
            var bytes = File.ReadAllBytes(keyIdentifier);

            return Encoding.UTF8.GetString(bytes);
        }

    }
}
