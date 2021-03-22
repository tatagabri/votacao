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
using Votacao.Repository.Models.System;
using Votacao.Security.Models;

namespace Votacao.Controllers
{
    [Route("admin")]
    public class AdminController : VotacaoController
    {
        public AdminController(IServiceProvider provider) : base(provider) { }

        public async Task<IActionResult> Index([FromServices] IRepository<Election> repository, CancellationToken ct)
        {
            if (!IsAdminLogged(out IActionResult action)) return action;
            var runningElection = await repository.QueryScalarAsync(q => q.SingleOrDefaultAsync(x => x.Status == ElectionStatus.Running, ct), ct);
            TempData["HasRunningElection"] = runningElection != null;
            return await Task.FromResult(View());
        }
    }
}
