namespace Sdk.Crypto
{
    public interface IPersonalCryptoService
    {
        string Encrypt(string key, string value);
        string Decrypt(string key, string value);
    }
}
