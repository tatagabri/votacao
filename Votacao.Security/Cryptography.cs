using BCrypt.Net;

namespace Votacao.Security
{
    public static class Cryptography
    {
        public static string GenerateHash(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password, HashType.SHA384);
        }

        public static bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password, hash, HashType.SHA384);
        }
    }
}
