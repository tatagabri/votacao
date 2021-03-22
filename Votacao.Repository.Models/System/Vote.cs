using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Votacao.Repository.Models.Security;

namespace Votacao.Repository.Models.System
{
    [Table("SYS_Vote")]
    public class Vote : BaseEntity
    {
        public virtual int ElectionId { get; set; }
        public virtual Election Election { get; set; }
        public virtual int VoterId { get; set; }
        public virtual Identity Voter { get; set; }
        public virtual int? CandidateId { get; set; }
        public virtual Candidate Candidate { get; set; }
    }
}
