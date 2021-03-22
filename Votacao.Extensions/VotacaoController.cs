using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Votacao.Security.Models;

namespace Votacao.Extensions
{
    public class VotacaoController : Controller
    {
        protected IServiceProvider Provider { get; }

        protected IUserContext UserContext { get; }

        protected MessageViewModel NotAllowedModel { get; }

        protected VotacaoController(IServiceProvider provider) : base()
        {
            Provider = provider;
            UserContext = provider.GetRequiredService<IUserContext>();
            NotAllowedModel = new MessageViewModel
            {
                Title = "Acesso negado!",
                Message = "Você não tem acesso a essa página.",
                RedirectTo = "/",
                Success = false
            };
        }

        protected bool IsAdminLogged(out IActionResult action)
        {
            if (!string.IsNullOrWhiteSpace(UserContext.Principal) && UserContext.IsAdmin)
            {
                action = Ok();
                return true;
            }
            else
            {
                action = View("Message", NotAllowedModel);
                return false;
            }
        }

        protected bool IsMemberLogged(out IActionResult action)
        {
            if (!string.IsNullOrWhiteSpace(UserContext.Principal) && !UserContext.IsAdmin)
            {
                action = Ok();
                return true;
            }
            else
            {
                action = View("Message", NotAllowedModel);
                return false;
            }
        }
    }
}
