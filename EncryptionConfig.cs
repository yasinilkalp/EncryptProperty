namespace EncryptProperty
{
    public class EncryptionConfig
    {
        public EncryptionConfig(string privateKey, bool isEncrypted)
        {
            PrivateKey = privateKey;
            IsEncrypted = isEncrypted;
        }
        public static bool IsEncrypted { get; set; }
        public static string PrivateKey { get; set; }


        public static EncryptionConfig SetConfig(string privateKey, bool isEncrypted) => new(privateKey, isEncrypted);
    }
}
