using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Votacao.Repository.Models.System
{
    [Table("SYS_Candidate")]
    public class Candidate : BaseEntity
    {
        [DataType("varchar")]
        [Required]
        [StringLength(30)]
        public string FirstName { get; set; }

        [DataType("varchar")]
        [Required]
        [StringLength(80)]
        public string LastName { get; set; }

        [NotMapped]
        public string FullName { get => (FirstName ?? "") + " " + (LastName ?? ""); }

        public int Code { get; set; }

        [DataType("varchar")]
        [StringLength(255)]
        public string Party { get; set; }

        [DataType("varchar")]
        [StringLength(3000)]
        public string ImageUrl { get; set; }
    }
}
