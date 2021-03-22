using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Votacao.Extensions;
using Votacao.Models;
using Votacao.Repository;
using Votacao.Repository.Models.Security;
using Votacao.Security;
using Votacao.Security.Models;

namespace Votacao.Controllers
{
    [Route("identity")]
    public class IdentityController : VotacaoController
    {
        public IdentityController(IServiceProvider provider) : base(provider) { }

        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            if (!IsAdminLogged(out IActionResult action)) return action;
            return await Task.FromResult(View("Create"));
        }

        [HttpPost("create")]
        public async Task<IActionResult> PostCreate(
            [FromForm] Identity model, 
            [FromServices] IRepository<Identity> repository,
            [FromServices] IUserContext userContext,
            CancellationToken ct)
        {
            if (!IsAdminLogged(out IActionResult action)) return action;
            model.UserName = model.Email;
            model.NormalizedEmail = model.Email.ToLower();
            model.NormalizedUserName = model.Email.ToLower();
            model.Enabled = true;
            model.EmailConfirmed = true;
            model.PasswordHash = Cryptography.GenerateHash(model.Password);
            try
            {
                await repository.AddAsync(model, userContext, ct);
                await repository.SaveAsync(ct);
                return View("Message", new MessageViewModel
                {
                    Title = "",
                    Message = "Eleitor criado com sucesso!",
                    RedirectTo = "/admin",
                    Success = true
                });
            }
            catch (ServiceException ex)
            {
                return View("Message", new MessageViewModel
                {
                    Title = ex.Title,
                    Message = ex.Message,
                    RedirectTo = "/admin",
                    Success = false
                });
            }
        }

        [HttpGet("list")]
        public async Task<IActionResult> List(
            [FromServices] IRepository<Identity> identityRepository,
            [FromServices] IRepository<Role> roleRepository,
            CancellationToken ct)
        {
            if (!IsAdminLogged(out IActionResult action)) return action;
            var adminRole = await roleRepository.QueryScalarAsync(q => q.SingleOrDefaultAsync(x => x.NormalizedName == "admin", ct), ct);
            var adminIds = adminRole.Identities.Select(x => x.UserId);
            var result = await identityRepository.QueryAsync(q => q.Where(x => !adminIds.Contains(x.Id)), null, ct);
            return await Task.FromResult(View("List", result.ToArray()));
        }

        [HttpPost("list")]
        public async Task<IActionResult> PostList(
            [FromForm(Name = "search")] string search,
            [FromServices] IRepository<Identity> identityRepository,
            [FromServices] IRepository<Role> roleRepository,
            CancellationToken ct)
        {
            if (!IsAdminLogged(out IActionResult action)) return action;
            var adminRole = await roleRepository.QueryScalarAsync(q => q.SingleOrDefaultAsync(x => x.NormalizedName == "admin", ct), ct);
            var adminIds = adminRole.Identities.Select(x => x.UserId);
            var result = await identityRepository.QueryAsync(q => q.Where(x => !adminIds.Contains(x.Id)), null, ct);
            if (!string.IsNullOrWhiteSpace(search))
            {
                result = result.Where(x =>
                    x.FullName.ToLower().Contains(search.ToLower()) ||
                    x.Email.ToLower().Contains(search.ToLower()) ||
                    x.CPF == search ||
                    (search.ToLower() == "ativo" && x.Enabled) ||
                    (search.ToLower() == "inativo" && !x.Enabled)
                );
            }
            return await Task.FromResult(View("List", result.ToArray()));
        }

        [HttpGet("update/{id:int}")]
        public async Task<IActionResult> Update(
            [FromRoute] int id, 
            [FromServices] IRepository<Identity> repository,
            CancellationToken ct)
        {
            if (!IsAdminLogged(out IActionResult action)) return action;
            var identity = await repository.QueryByIdAsync(id, ct);
            return await Task.FromResult(View("Update", identity));
        }

        [HttpPost("update/{id:int}")]
        public async Task<IActionResult> PostUpdate(
            [FromRoute] int id,
            [FromForm] Identity model,
            [FromServices] IRepository<Identity> repository,
            [FromServices] IUserContext userContext,
            CancellationToken ct)
        {
            if (!IsAdminLogged(out IActionResult action)) return action;

            model.PasswordHash = Cryptography.GenerateHash(model.Password);

            var identity = await repository.QueryByIdAsync(id, ct);
            identity.FirstName = model.FirstName;
            identity.LastName = model.LastName;
            identity.CPF = model.CPF;
            identity.Email = model.Email;
            identity.UserName = model.Email;
            identity.NormalizedEmail = model.Email.ToLower();
            identity.NormalizedUserName = model.Email.ToLower();
            identity.Enabled = model.Enabled;
            if (model.Password != "aaaaaaaaa") identity.PasswordHash = model.PasswordHash;

            try
            {
                await repository.UpdateAsync(identity, userContext, ct);
                await repository.SaveAsync(ct);

                return View("Message", new MessageViewModel
                {
                    Title = "",
                    Message = "Eleitor alterado com sucesso!",
                    RedirectTo = "/admin",
                    Success = true
                });
            }
            catch (ServiceException ex)
            {
                return View("Message", new MessageViewModel
                {
                    Title = ex.Title,
                    Message = ex.Message,
                    RedirectTo = "/admin",
                    Success = false
                });
            }
        }
    }
}
