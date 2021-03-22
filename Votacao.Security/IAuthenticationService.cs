using System.Threading.Tasks;
using Votacao.Security.Models;

namespace Votacao.Security
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResult> AuthenticateAsync(BaseResult<AuthenticationIdentity> result);
    }
}
