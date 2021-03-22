using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Votacao.Repository.Models
{
    public interface IEntity
    {
        int Id { get; set; }
        string CreationUser { get; set; }
        string EditionUser { get; set; }
        string CreationIp { get; set; }
        string EditionIp { get; set; }
        DateTime? CreationDate { get; set; }
        DateTime? EditionDate { get; set; }
    }
}
