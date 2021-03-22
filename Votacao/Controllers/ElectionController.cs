using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    [Route("election")]
    public class ElectionController : VotacaoController
    {
        public ElectionController(IServiceProvider provider) : base(provider) { }

        public class Votes
        {
            public IDictionary<int, Candidate> Candidates { get; set; }
            public IDictionary<int, int> VoteCounts { get; set; }
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create([FromServices] IRepository<Candidate> repository, CancellationToken ct)
        {
            if (!IsAdminLogged(out IActionResult action)) return action;
            var candidates = await repository.QueryAsync(q => q, null, ct);
            return await Task.FromResult(View("Create", candidates.ToArray()));
        }

        [HttpPost("create")]
        public async Task<IActionResult> PostCreate(
            [FromForm(Name = "name")] string name,
            [FromForm(Name = "candidates")] IEnumerable<int> candidates,
            [FromServices] IRepository<Election> repository,
            CancellationToken ct)
        {
            if (!IsAdminLogged(out IActionResult action)) return action;

            var model = new Election()
            {
                Name = name,
                Candidates = candidates.Select(x => new ElectionCandidate { CandidateId = x }).ToList(),
                Status = ElectionStatus.Scheduled,
                Votes = new List<Vote>()
            };

            try
            {
                await repository.AddAsync(model, UserContext, ct);
                await repository.SaveAsync(ct);
                return View("Message", new MessageViewModel
                {
                    Title = "",
                    Message = "Eleição criada com sucesso!",
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
            [FromServices] IRepository<Election> repository,
            CancellationToken ct)
        {
            if (!IsAdminLogged(out IActionResult action)) return action;
            var elections = await repository.QueryAsync(q => q, o => o.OrderBy(x => x.Status == ElectionStatus.Running ? 0 : 1), ct);
            return View("List", elections.ToArray());
        }

        [HttpPost("list")]
        public async Task<IActionResult> PostList(
            [FromForm(Name = "search")] string search,
            [FromServices] IRepository<Election> repository,
            CancellationToken ct)
        {
            if (!IsAdminLogged(out IActionResult action)) return action;
            var result = await repository.QueryAsync(q => q, o => o.OrderBy(x => x.Status == ElectionStatus.Running ? 0 : 1), ct);
            if (!string.IsNullOrWhiteSpace(search))
            {
                result = result.Where(x =>
                    x.Name.ToLower().Contains(search.ToLower()) ||
                    x.StartDate.ToString().ToLower().Contains(search.ToLower()) ||
                    x.EndDate.ToString().ToLower().Contains(search.ToLower()) ||
                    (search.ToLower() == "agendada" && x.Status == ElectionStatus.Scheduled) ||
                    (search.ToLower() == "execução" && x.Status == ElectionStatus.Running) ||
                    (search.ToLower() == "execucao" && x.Status == ElectionStatus.Running) ||
                    (search.ToLower() == "em execução" && x.Status == ElectionStatus.Running) ||
                    (search.ToLower() == "em execucao" && x.Status == ElectionStatus.Running) ||
                    (search.ToLower() == "cancelada" && x.Status == ElectionStatus.Cancelled) ||
                    (search.ToLower() == "finalizada" && x.Status == ElectionStatus.Finished)
                );
            }
            return View("List", result.ToArray());
        }

        [HttpGet("active")]
        public async Task<IActionResult> Active(
            [FromServices] IRepository<Election> repository,
            CancellationToken ct)
        {
            if (!IsAdminLogged(out IActionResult action)) return action;
            var runningElection = await repository.QueryScalarAsync(q => q.SingleOrDefaultAsync(x => x.Status == ElectionStatus.Running, ct), ct);
            if (runningElection != null)
                return View("Update", runningElection);
            else
                return View("Message", new MessageViewModel
                {
                    Title = "",
                    Message = "Não existe uma eleição ativa no momento!",
                    RedirectTo = "/admin",
                    Success = false
                });

        }

        [HttpGet("update/{id:int}")]
        public async Task<IActionResult> Update(
            [FromRoute] int id,
            [FromServices] IRepository<Election> repository,
            CancellationToken ct)
        {
            if (!IsAdminLogged(out IActionResult action)) return action;
            var election = await repository.QueryByIdAsync(id, ct);
            return View("Update", election);
        }

        [HttpGet("update/{id:int}/{status:int}")]
        public async Task<IActionResult> UpdateStatus(
            [FromRoute] int id,
            [FromRoute] int status,
            [FromServices] IRepository<Election> repository,
            CancellationToken ct)
        {
            if (!IsAdminLogged(out IActionResult action)) return action;
            var newStatus = (ElectionStatus)status;
            if (newStatus == ElectionStatus.Running)
            {
                var runningElection = await repository.QueryScalarAsync(q => q.SingleOrDefaultAsync(x => x.Status == ElectionStatus.Running, ct), ct);
                if (runningElection != null)
                {
                    return View("Message", new MessageViewModel
                    {
                        Title = "",
                        Message = "Já existe uma eleição em progresso no momento!",
                        RedirectTo = "/admin",
                        Success = false
                    });
                }
            }

            var election = await repository.QueryByIdAsync(id, ct);
            election.Status = newStatus;

            if (newStatus == ElectionStatus.Running)
            {
                election.StartDate = DateTime.Now;
            } 
            else if (newStatus == ElectionStatus.Finished || newStatus == ElectionStatus.Cancelled)
            {
                election.EndDate = DateTime.Now;
            }
            else
            {
                return View("Message", new MessageViewModel
                {
                    Title = "",
                    Message = "Status de eleição inválido",
                    Success = false,
                    RedirectTo = "/election/update/" + id
                });
            }

            try
            {
                await repository.UpdateAsync(election, UserContext, ct);
                await repository.SaveAsync(ct);

                if (newStatus == ElectionStatus.Finished)
                {
                    return View("Message", new MessageViewModel
                    {
                        Title = "",
                        Message = "Status da eleição alterado com sucesso!",
                        RedirectTo = "/election/counting/" + id,
                        Success = true
                    });
                }

                return View("Message", new MessageViewModel
                {
                    Title = "",
                    Message = "Status da eleição alterado com sucesso!",
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

        [HttpGet("counting/{id:int}")]
        public async Task<IActionResult> Counting(
            [FromRoute] int id,
            [FromServices] IRepository<Election> repository,
            CancellationToken ct)
        {
            if (!IsAdminLogged(out IActionResult action)) return action;
            var election = await repository.QueryByIdAsync(id, ct);

            var votes = election.Votes.Aggregate(new Dictionary<int, int>(), (a, v) =>
            {
                if (a.ContainsKey(v.CandidateId ?? -1))
                {
                    a[v.CandidateId ?? -1] += 1;
                }
                else
                {
                    a.Add(v.CandidateId ?? -1, 1);
                }

                return a;
            }, a => a);

            foreach (var candidate in election.Candidates)
            {
                if (!votes.ContainsKey(candidate.CandidateId))
                {
                    votes.Add(candidate.CandidateId, 0);
                }
            }

            if (!votes.ContainsKey(-1)) votes.Add(-1, 0);

            var candidates = election.Candidates.ToDictionary(x => x.CandidateId, x => x.Candidate).OrderByDescending(x => votes[x.Key]).ToDictionary(x => x.Key, x => x.Value);

            return View("Counting", new Votes
            {
                Candidates = candidates,
                VoteCounts = votes
            });
        }

    }
}
