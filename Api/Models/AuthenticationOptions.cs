namespace dordle.common.models.authentication
{
    public class AuthenticationOptions
    {
        public const string ConfigKey = nameof(AuthenticationOptions);

        public string Authority { get; set; } = string.Empty;
        public string Realm { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string ScopedEntity { get; set; } = string.Empty;

        public string AuthorizationUrl
        {
            get
            {
                return $"{Authority}/realms/{Realm}/protocol/openid-connect/auth";
            }
            private set { }
        }
        public string TokenUrl
        {
            get
            {
                return $"{Authority}/realms/{Realm}/protocol/openid-connect/auth";
            }
            private set { }
        }

        public string MetadataAddress
        {
            get
            {
                return $"{Authority}/realms/{Realm}/.well-known/openid-configuration";
            }
            private set { }
        }
        public string ValidIssuer
        {
            get
            {
                return $"{Authority}/realms/{Realm}";
            }
            private set { }
        }



    }
}
