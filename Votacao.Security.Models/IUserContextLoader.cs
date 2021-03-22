using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Votacao.Security.Models
{
    public interface IUserContextLoader
    {
        void Load(IUserContext userContext);
    }
}
