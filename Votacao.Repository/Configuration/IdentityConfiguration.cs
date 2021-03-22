using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Votacao.Repository.Models.Security;

namespace Votacao.Repository.Configuration
{
    public class IdentityConfiguration : IEntityTypeConfiguration<Identity>
    {
        public void Configure(EntityTypeBuilder<Identity> builder)
        {
            builder.HasIndex(x => x.CPF).IsUnique();
            builder.HasIndex(x => x.Email).IsUnique();
        }
    }
}
