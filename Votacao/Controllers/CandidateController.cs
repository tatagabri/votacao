using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Votacao.Extensions;
using Votacao.Repository;
using Votacao.Repository.Models.System;

namespace Votacao.Controllers
{
    [Route("candidate")]
    public class CandidateController : VotacaoController
    {
        public CandidateController(IServiceProvider provider) : base(provider) { }

        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            if (!IsAdminLogged(out IActionResult action)) return action;
            return await Task.FromResult(View("Create"));
        }

        [HttpPost("create")]
        public async Task<IActionResult> PostCreate(
            [FromForm] Candidate model,
            [FromServices] IRepository<Candidate> repository,
            CancellationToken ct)
        {
            if (!IsAdminLogged(out IActionResult action)) return action;

            try
            {
                await repository.AddAsync(model, UserContext, ct);
                await repository.SaveAsync(ct);
                return View("Message", new MessageViewModel
                {
                    Title = "",
                    Message = "Candidato criado com sucesso!",
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

        [HttpGet("update/{id:int}")]
        public async Task<IActionResult> Update(
            [FromRoute] int id,
            [FromServices] IRepository<Candidate> repository,
            CancellationToken ct)
        {
            if (!IsAdminLogged(out IActionResult action)) return action;
            var candidate = await repository.QueryByIdAsync(id, ct);
            return await Task.FromResult(View("Update", candidate));
        }

        [HttpPost("update/{id:int}")]
        public async Task<IActionResult> PostUpdate(
            [FromRoute] int id,
            [FromForm] Candidate model,
            [FromServices] IRepository<Candidate> repository,
            CancellationToken ct)
        {
            if (!IsAdminLogged(out IActionResult action)) return action;

            try
            {
                var candidate = await repository.QueryByIdAsync(id, ct);
                candidate.Code = model.Code;
                candidate.FirstName = model.FirstName;
                candidate.LastName = model.LastName;
                candidate.ImageUrl = model.ImageUrl;
                candidate.Party = model.Party;

                await repository.UpdateAsync(candidate, UserContext, ct);
                await repository.SaveAsync(ct);
                return View("Message", new MessageViewModel
                {
                    Title = "",
                    Message = "Candidato alterado com sucesso!",
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
            [FromServices] IRepository<Candidate> repository,
            CancellationToken ct)
        {
            if (!IsAdminLogged(out IActionResult action)) return action;
            var result = await repository.QueryAsync(q => q, null, ct);
            return await Task.FromResult(View("List", result.ToArray()));
        }

        [HttpPost("list")]
        public async Task<IActionResult> PostList(
            [FromForm(Name = "search")] string search,
            [FromServices] IRepository<Candidate> repository,
            CancellationToken ct)
        {
            if (!IsAdminLogged(out IActionResult action)) return action;
            var result = await repository.QueryAsync(q => q, null, ct);
            if (!string.IsNullOrWhiteSpace(search))
            {
                result = result.Where(x =>
                    x.FullName.ToLower().Contains(search.ToLower()) ||
                    x.Party.ToLower().Contains(search.ToLower()) ||
                    (int.TryParse(search, out int nsearch) && x.Code == nsearch)
                );
            }
            return await Task.FromResult(View("List", result.ToArray()));
        }

        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(
            [FromRoute] int id,
            [FromServices] IRepository<Candidate> repository,
            CancellationToken ct)
        {
            try
            {
                if (!IsAdminLogged(out IActionResult action)) throw new ServiceException(NotAllowedModel.Title, NotAllowedModel.Message);
                await repository.DeleteAsync(id, UserContext, ct);
                return Ok();
            }
            catch (ServiceException ex)
            {
                return BadRequest(new MessageViewModel
                {
                    Title = ex.Title,
                    Message = ex.Message,
                    RedirectTo = "/admin",
                    Success = false
                });
            }
        }

        [HttpGet("deletesuccess")]
        public async Task<IActionResult> DeleteSuccess()
        {
            if (!IsAdminLogged(out IActionResult action)) return action;
            return await Task.FromResult(View("Message", new MessageViewModel
            {
                Title = "",
                Message = "Candidato excluido com sucesso!",
                RedirectTo = "/admin",
                Success = true
            }));
        }

        [HttpGet("deletefail")]
        public async Task<IActionResult> DeleteFail(
            [FromQuery] string title,
            [FromQuery] string message,
            [FromQuery] string redirectto,
            [FromQuery] bool success)
        {
            if (!IsAdminLogged(out IActionResult action)) return action;
            return await Task.FromResult(View("Message", new MessageViewModel
            {
                Title = title,
                Message = message,
                RedirectTo = redirectto,
                Success = success
            }));
        }
    }
}
