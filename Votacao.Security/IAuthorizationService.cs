using System.Threading;
using System.Threading.Tasks;
using Votacao.Security.Models;

namespace Votacao.Security
{
    public interface IAuthorizationService
    {
        public Task<BaseResult<AuthenticationIdentity>> AuthorizeAsync(LoginUser loginUser, CancellationToken ct = default);
    }
}
