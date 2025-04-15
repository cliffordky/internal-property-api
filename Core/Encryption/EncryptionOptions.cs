namespace Core.Encryption
{
    public class EncryptionOptions
    {
        public const string ConfigKey = nameof(EncryptionOptions);
        public string Key { get; set; } = string.Empty;
        public string Salt { get; set; } = string.Empty;
    }
}
