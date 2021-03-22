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
    public class ElectionCandidateConfiguration : IEntityTypeConfiguration<ElectionCandidate>
    {
        public void Configure(EntityTypeBuilder<ElectionCandidate> builder)
        {
            builder.HasKey(x => new { x.CandidateId, x.ElectionId });

            builder
                .HasOne(x => x.Election)
                .WithMany(x => x.Candidates)
                .HasForeignKey(x => x.ElectionId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(x => x.Candidate)
                .WithMany()
                .HasForeignKey(x => x.CandidateId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
