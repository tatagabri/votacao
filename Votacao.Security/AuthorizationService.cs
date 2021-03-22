using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Votacao.Repository;
using Votacao.Repository.Models.Security;
using Votacao.Repository.Models.Security.Relations;
using Votacao.Security.Models;

namespace Votacao.Security
{
    public sealed class AuthorizationService : IAuthorizationService
    {
        private IServiceProvider Provider { get; }

        public AuthorizationService(IServiceProvider provider)
        {
            Provider = provider.CreateScope().ServiceProvider;
        }

        public async Task<BaseResult<AuthenticationIdentity>> AuthorizeAsync(LoginUser loginUser, CancellationToken ct = default)
        {
            var username = loginUser?.Username ?? "";
            var password = loginUser?.Password ?? "";

            var repo = Provider.GetService<IRepository<Identity>>();
            var roleRepo = Provider.GetService<IRepository<Role>>();

            var identity = await repo.QueryScalarAsync(q => q.SingleOrDefaultAsync(x => x.UserName == username, ct), ct);
            // var passwordHash = Cryptography.GenerateHash(password);

            if (identity == null)
            {
                return new BaseResult<AuthenticationIdentity>()
                {
                    Message = "Usuário e/ou senha incorretos!",
                    Success = false
                };
            }

            if (!identity.Enabled)
            {
                return new BaseResult<AuthenticationIdentity>()
                {
                    Message = "Usuário desativado pelo administrador!",
                    Success = false
                };
            }

            if (!Cryptography.VerifyPassword(password, identity.PasswordHash))
            {
                return new BaseResult<AuthenticationIdentity>()
                {
                    Message = "Usuário e/ou senha incorretos!",
                    Success = false
                };
            }

            var role = await roleRepo.QueryScalarAsync(q => q.SingleOrDefaultAsync(x => x.NormalizedName == "admin", ct), ct);

            return new BaseResult<AuthenticationIdentity>()
            {
                Message = "Logado com sucesso!",
                Success = true,
                Data = new AuthenticationIdentity
                {
                    IdentityId = identity.Id,
                    Email = identity.Email,
                    FullName = identity.FirstName + ' ' + identity.LastName,
                    IsAdmin = role.Identities.Any(x => x.UserId == identity.Id)
                }
            };
        }
    }
}
