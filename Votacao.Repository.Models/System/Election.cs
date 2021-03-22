using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Votacao.Repository.Models.System
{
    public enum ElectionStatus
    {
        Scheduled, Running, Cancelled, Finished
    }

    [Table("SYS_Election")]
    public class Election : BaseEntity
    {
        [DataType("varchar")]
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public ElectionStatus Status { get; set; }

        public virtual IList<ElectionCandidate> Candidates { get; set; }

        public virtual IList<Vote> Votes { get; set; }
    }
}
