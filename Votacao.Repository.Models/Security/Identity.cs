using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Votacao.Repository.Models.Security
{
    [Table("SEC_Identity")]
    public class Identity : IdentityUser<int>, IEntity
    {
        #region BaseEntity
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }

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
        #endregion

        [Required]
        [DataType("varchar")]
        [StringLength(30)]
        public string FirstName { get; set; }

        [Required]
        [DataType("varchar")]
        [StringLength(80)]
        public string LastName { get; set; }

        [NotMapped]
        public string FullName { get => (FirstName ?? "") + " " + (LastName ?? ""); }

        [Required]
        [DataType("varchar")]
        [StringLength(15)]
        public string CPF { get; set; }

        [NotMapped]
        public string Password { get; set; }

        public bool Enabled { get; set; }
    }
}
