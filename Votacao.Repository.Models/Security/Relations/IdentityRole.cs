using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Votacao.Repository.Models.Security.Relations
{
    [Table("SEC_IdentityRole")]
    public class IdentityRole : IdentityUserRole<int>
    {
    }
}
