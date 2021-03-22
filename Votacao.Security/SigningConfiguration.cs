using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Votacao.Security
{
    public sealed class SigningConfiguration
    {
        private UTF8Encoding UTF8 { get; }
        public SecurityKey Key { get; }
        public SigningCredentials SigningCredentials { get; }

        public SigningConfiguration(TokenConfiguration tokenConfiguration)
        {
            UTF8 = new UTF8Encoding();
            Key = new SymmetricSecurityKey(UTF8.GetBytes(tokenConfiguration.SecretKey));
            SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
        }
    }
}
