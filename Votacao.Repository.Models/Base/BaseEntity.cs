using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Votacao.Repository.Models
{
    public class BaseEntity : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DataType("varchar")]
        [StringLength(50)]
        [ScaffoldColumn(false)]
        public string CreationUser { get; set; }

        [DataType("varchar")]
        [StringLength(50)]
        [ScaffoldColumn(false)]
        public string EditionUser { get; set; }

        [StringLength(50)]
        [DataType("varchar")]
        [ScaffoldColumn(false)]
        public string CreationIp { get; set; }

        [StringLength(50)]
        [DataType("varchar")]
        [ScaffoldColumn(false)]
        public string EditionIp { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? CreationDate { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? EditionDate { get; set; }
    }
}
