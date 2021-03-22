using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace Votacao.Security.Models
{
    public sealed class UserContextLoader : IUserContextLoader
    {
        private IServiceProvider Provider { get; }

        public UserContextLoader(IServiceProvider provider)
        {
            Provider = provider;
        }

        private AuthenticationIdentity GetTokenObject(HttpContext httpContext)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = httpContext.Session.GetString("access_token");
            if (token == null || !handler.CanReadToken(token)) return null;
            var tokenobj = handler.ReadJwtToken(token);
            if (tokenobj.ValidTo > DateTime.Now)
            {
                var data = tokenobj.Claims.FirstOrDefault(x => x.Type == "data");
                return JsonConvert.DeserializeObject<AuthenticationIdentity>(data.Value);
            }
            return null;
        }

        public void Load(IUserContext userContext)
        {
            try
            {
                var httpContext = Provider.GetRequiredService<IHttpContextAccessor>().HttpContext;
                if (httpContext == null)
                    return;

                var token = GetTokenObject(httpContext);

                if (token != null)
                {
                    userContext.Principal = token.Email;
                    userContext.IsAdmin = token.IsAdmin;
                }

                if (httpContext.Request != null)
                {
                    userContext.IP = httpContext.Connection.RemoteIpAddress.ToString();
                    userContext.HostName = httpContext.Connection.RemoteIpAddress.ToString();
                }
            }
            catch { }
        }
    }
}
