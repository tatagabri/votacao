using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

namespace Votacao.Security.Models
{
    public sealed class UserContext : IUserContext
    {
        public string Principal { get; set; }
        public string IP { get; set; }
        public string HostName { get; set; }
        public bool IsAdmin { get; set; }
    }
}
