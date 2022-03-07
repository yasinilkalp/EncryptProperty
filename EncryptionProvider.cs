using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace EncryptProperty
{
    public class EncryptionProvider : IEncryptionProvider
    {
        private readonly static string PrivateKey = EncryptionConfig.PrivateKey;
        private readonly static bool IsEncrypted = EncryptionConfig.IsEncrypted;

        public string Encrypt(string value)
        {
            if (!IsEncrypted) return value;

            if (string.IsNullOrEmpty(PrivateKey))
                throw new ArgumentNullException("EncryptionKey", "Please initialize your encryption key.");

            if (string.IsNullOrEmpty(value))
                return string.Empty;

            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(PrivateKey);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using MemoryStream memoryStream = new();
                using CryptoStream cryptoStream = new(memoryStream, encryptor, CryptoStreamMode.Write);
                using (StreamWriter streamWriter = new(cryptoStream))
                {
                    streamWriter.Write(value);
                }
                array = memoryStream.ToArray();
            }
            string result = Convert.ToBase64String(array);
            return result;
        }

        public string Decrypt(string value)
        {
            try
            {

                if (!IsEncrypted) return value;

                if (string.IsNullOrEmpty(PrivateKey))
                    throw new ArgumentNullException("EncryptionKey", "Please initialize your encryption key.");

                if (string.IsNullOrEmpty(value))
                    return string.Empty;

                byte[] iv = new byte[16];

                using Aes aes = Aes.Create();
                aes.Key = Encoding.UTF8.GetBytes(PrivateKey);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                var buffer = Convert.FromBase64String(value);

                using MemoryStream memoryStream = new(buffer);

                using CryptoStream cryptoStream = new(memoryStream, decryptor, CryptoStreamMode.Read);

                using StreamReader streamReader = new(cryptoStream);

                return streamReader.ReadToEnd();
            }
            catch
            {
                return value;
            }

        }
    }
}
