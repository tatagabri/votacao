using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Votacao.Extensions;
using Votacao.Models;
using Votacao.Security.Models;

namespace Votacao.Controllers
{
    [AllowAnonymous]
    public class HomeController : VotacaoController
    {
        public HomeController(IServiceProvider provider) : base(provider) { }

        public async Task<IActionResult> Index()
        {
            return await Task.FromResult(View());
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromForm(Name = "email")] string email, 
            [FromForm(Name = "password")] string password, 
            CancellationToken ct)
        {
            var authorizationService = Provider.GetRequiredService<Security.IAuthorizationService>();
            var authorizationResult = await authorizationService.AuthorizeAsync(new LoginUser()
            {
                Username = email,
                Password = password
            }, ct);

            var authService = Provider.GetRequiredService<Security.IAuthenticationService>();
            var result = await authService.AuthenticateAsync(authorizationResult);

            if (result.Authenticated)
            {
                HttpContext.Session.SetString("access_token", result.AccessToken);
                return RedirectToAction("Index", "Member");
            }
            else
            {
                HttpContext.Session.Clear();
                TempData["ErrorMessage"] = result.Message;
                return View("LoginFail");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
