using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Votacao.Repository.Models.System;

namespace Votacao.Repository.Configuration
{
    public class VoteConfiguration : IEntityTypeConfiguration<Vote>
    {
        public void Configure(EntityTypeBuilder<Vote> builder)
        {
            builder.HasOne(x => x.Election).WithMany(x => x.Votes).HasForeignKey(x => x.ElectionId)
                .IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Voter).WithMany().HasForeignKey(x => x.VoterId)
                .IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Candidate).WithMany().HasForeignKey(x => x.CandidateId)
                .IsRequired(false).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
