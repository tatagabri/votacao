using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Votacao.Repository.Models.System
{
    [Table("SYS_ElectionCandidate")]
    public class ElectionCandidate
    {
        public virtual int ElectionId { get; set; }
        public virtual Election Election { get; set; }
        public virtual int CandidateId { get; set; }
        public virtual Candidate Candidate { get; set; }
    }
}
