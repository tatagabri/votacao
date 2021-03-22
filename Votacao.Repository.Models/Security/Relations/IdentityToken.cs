using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Votacao.Repository.Models.Security.Relations
{
    [Table("SEC_IdentityToken")]
    public class IdentityToken : IdentityUserToken<int>, IEntity
    {
        public override int UserId { get => this.Id; set => this.Id = value; }

        public virtual int Id { get; set; }

        [DataType("varchar")]
        [StringLength(50)]
        [ScaffoldColumn(false)]
        public string EditionUser { get; set; }

        [DataType("varchar")]
        [StringLength(50)]
        [ScaffoldColumn(false)]
        public string CreationUser { get; set; }

        [DataType("varchar")]
        [StringLength(50)]
        [ScaffoldColumn(false)]
        public string EditionIp { get; set; }

        [DataType("varchar")]
        [StringLength(50)]
        [ScaffoldColumn(false)]
        public string CreationIp { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? EditionDate { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? CreationDate { get; set; }
    }
}
