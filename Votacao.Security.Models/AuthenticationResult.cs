namespace Votacao.Security.Models
{
    public class AuthenticationResult
    {
        public bool Authenticated { get; set; }
        public string CreatedAt { get; set; }
        public string Expiration { get; set; }
        public string AccessToken { get; set; }
        public string Message { get; set; }
    }
}
