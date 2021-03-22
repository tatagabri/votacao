using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

namespace Votacao.Security.Models
{
    public interface IUserContext
    {
        string Principal { get; set; }
        string IP { get; set; }
        string HostName { get; set; }
        bool IsAdmin { get; set; }
    }
}
