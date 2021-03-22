namespace Votacao.Security
{
    public sealed class TokenConfiguration
    {
        public const string Policy = "Bearer";
        public string SecretKey { get; set; }
        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
        public int ExpirationInSeconds { get; set; }
        public bool ValidateLifetime { get; set; }
        public bool ValidateIssuerSigningKey { get; set; }
    }
}
