namespace Votacao.Security.Models
{
    public sealed class AuthenticationIdentity
    {
        public int IdentityId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public bool IsAdmin { get; set; }
    }
}
