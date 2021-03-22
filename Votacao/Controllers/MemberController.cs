using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Votacao.Extensions;
using Votacao.Repository;
using Votacao.Repository.Models.Security;
using Votacao.Repository.Models.System;
using Votacao.Security.Models;

namespace Votacao.Controllers
{
    [Route("member")]
    public class MemberController : VotacaoController
    {
        public MemberController(IServiceProvider provider) : base(provider) { }

        public class MemberViewModel
        {
            public Election Election { get; set; }
            public Candidate Candidate { get; set; }
        }

        public async Task<IActionResult> Index(
            [FromQuery(Name = "code")] int code,
            [FromQuery(Name = "candidateid")] int candidateid,
            [FromServices] IRepository<Election> repository,
            [FromServices] IRepository<Vote> voteRepository,
            [FromServices] IRepository<Identity> identityRepository,
            CancellationToken ct)        
        {
            if (IsAdminLogged(out IActionResult action)) return await Task.FromResult(RedirectToAction("Index", "Admin"));
            if (!IsMemberLogged(out action)) return action;

            var runningElection = await repository.QueryScalarAsync(q => q.SingleOrDefaultAsync(x => x.Status == ElectionStatus.Running, ct), ct);

            if (runningElection == null)
            {
                return View("Message", new MessageViewModel
                {
                    Title = "",
                    Message = "Não existe nenhuma eleição ocorrendo no memento!",
                    Success = false,
                    RedirectTo = "/"
                });
            }

            if (runningElection.Votes.Any(x => x.Voter.Email == UserContext.Principal))
            {
                return View("Message", new MessageViewModel
                {
                    Title = "",
                    Message = "Você já fez o seu voto nessa eleição!",
                    Success = false,
                    RedirectTo = "/"
                });
            }

            var model = new MemberViewModel
            {
                Election = runningElection
            };

            if (code > 0 && candidateid == 0)
            {
                model.Candidate = runningElection.Candidates.FirstOrDefault(x => x.Candidate.Code == code)?.Candidate;
                if (model.Candidate == null)
                {
                    return View("Message", new MessageViewModel
                    {
                        Title = "",
                        Message = "Esse candidato não existe! Tente de novo.",
                        RedirectTo = "/member",
                        Success = false
                    });
                }
            }
            else if (code > 0 && candidateid > 0)
            {
                try
                {
                    var user = await identityRepository.QueryScalarAsync(q => q.FirstOrDefaultAsync(x => x.Email == UserContext.Principal, ct), ct);
                    await voteRepository.AddAsync(new Vote
                    {
                        ElectionId = model.Election.Id,
                        CandidateId = candidateid,
                        VoterId = user.Id
                    }, UserContext, ct);
                    await voteRepository.SaveAsync(ct);

                    return View("Message", new MessageViewModel
                    {
                        Title = "",
                        Message = "FIM",
                        Success = true,
                        RedirectTo = "/"
                    });
                }
                catch (ServiceException ex)
                {
                    return View("Message", new MessageViewModel
                    {
                        Title = ex.Title,
                        Message = ex.Message,
                        Success = false,
                        RedirectTo = "/"
                    });
                }
            }
            else if (code == -1)
            {
                try
                {
                    var user = await identityRepository.QueryScalarAsync(q => q.FirstOrDefaultAsync(x => x.Email == UserContext.Principal, ct), ct);
                    await voteRepository.AddAsync(new Vote
                    {
                        ElectionId = model.Election.Id,
                        CandidateId = null,
                        VoterId = user.Id
                    }, UserContext, ct);
                    await voteRepository.SaveAsync(ct);

                    return View("Message", new MessageViewModel
                    {
                        Title = "",
                        Message = "FIM",
                        Success = true,
                        RedirectTo = "/"
                    });
                }
                catch (ServiceException ex)
                {
                    return View("Message", new MessageViewModel
                    {
                        Title = ex.Title,
                        Message = ex.Message,
                        Success = false,
                        RedirectTo = "/"
                    });
                }
            }
            
            return await Task.FromResult(View("Index", model));
        }
    }
}
