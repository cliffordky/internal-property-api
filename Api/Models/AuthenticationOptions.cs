namespace dordle.common.models.authentication
{
    public class AuthenticationOptions
    {
        public const string ConfigKey = nameof(AuthenticationOptions);
        public string Authority { get; set; } = string.Empty;
        public string AuthorizationUrl { get; set; } = string.Empty;
        public string TokenUrl { get; set; } = string.Empty;
        public string MetadataAddress { get; set; } = string.Empty;
        public string ValidIssuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string[] Scopes { get; set; } = Array.Empty<string>();
        public string ProtectedEntity { get; set; } = string.Empty;
        
    }
}
