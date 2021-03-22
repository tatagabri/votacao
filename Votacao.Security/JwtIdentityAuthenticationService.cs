using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Votacao.Repository.Models.Security;
using Votacao.Security;
using Votacao.Security.Models;

namespace Votacao.Security
{
    public sealed class JwtIdentityAuthenticationService : IAuthenticationService
    {
        private const string dateFormat = "yyyy-MM-dd HH:mm:ss";

        private SigningConfiguration SigningConfiguration { get; }

        private TokenConfiguration TokenConfiguration { get; }

        public JwtIdentityAuthenticationService(SigningConfiguration signingConfiguration, TokenConfiguration tokenConfiguration)
        {
            SigningConfiguration = signingConfiguration;
            TokenConfiguration = tokenConfiguration;
        }

        public async Task<AuthenticationResult> AuthenticateAsync(BaseResult<AuthenticationIdentity> result)
        {
            return await Task.Run(() =>
            {
                try
                {
                    if (!result.Success) throw new Exception();

                    var claims = new List<Claim>()
                    {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, result.Data.Email),
                        new Claim("data", ToJson(result.Data))
                    };

                    var subject = new ClaimsIdentity(new GenericIdentity(result.Data.Email, "Login"), claims);

                    var created = DateTime.Now;
                    var expiration = created + TimeSpan.FromSeconds(TokenConfiguration.ExpirationInSeconds);
                    var handler = new JwtSecurityTokenHandler();
                    var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                    {
                        Issuer = TokenConfiguration.ValidIssuer,
                        Audience = TokenConfiguration.ValidAudience,
                        SigningCredentials = SigningConfiguration.SigningCredentials,
                        Subject = subject,
                        NotBefore = created,
                        Expires = expiration,
                    });

                    return new AuthenticationResult
                    {
                        Authenticated = true,
                        CreatedAt = created.ToString(dateFormat),
                        Expiration = expiration.ToString(dateFormat),
                        AccessToken = handler.WriteToken(securityToken),
                        Message = "OK"
                    };
                }
                catch
                {
                    return new AuthenticationResult
                    {
                        Authenticated = false,
                        Message = result.Message
                    };
                }
            });    
        }

        private static string ToJson<T>(T obj)
        {
            if (obj == null) return null;

            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }
    }
}
