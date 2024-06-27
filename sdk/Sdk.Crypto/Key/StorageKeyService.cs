using System.IO;

namespace Sdk.Crypto.Service.Key
{
    public interface IStorageKeyService
    {
        bool Save(string xml, string keyIdentifier);

        string ReadKey(string keyIdentifier);
    }

    public class StorageKeyService : IStorageKeyService
    {
        private readonly Stream _stream;

        public StorageKeyService(Stream stream)
        {
            _stream = stream;
        }
        public string ReadKey(string keyIdentifier)
        {
            return new StreamReader(_stream).ReadToEnd();
        }

        public bool Save(string xml, string keyIdentifier)
        {
            new StreamWriter(_stream).Write(xml);
            return true;
        }
    }
}
