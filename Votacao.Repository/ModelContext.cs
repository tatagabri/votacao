using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Votacao.Repository.Models.Security;
using Votacao.Repository.Models.Security.Relations;
using Votacao.Repository.Models.System;

namespace Votacao.Repository
{
    public class ModelContext : IdentityDbContext<Identity, Role, int, IdentityClaim, IdentityRole, IdentityLogin, RoleClaim, IdentityToken>
    {
        public ModelContext(DbContextOptions<ModelContext> options) : base(options) { }        
        
        public DbSet<Election> Elections { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Vote> Votes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Carregar todas as configurações desse assembly
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            // Sobrescrição das tables padrões do Entity Framework Identity
            builder.Entity<Role>().HasMany(x => x.Identities).WithOne().HasForeignKey(x => x.RoleId);
            builder.Entity<Identity>().HasMany<IdentityRole>().WithOne().HasForeignKey(x => x.UserId);
            builder.Entity<Identity>().ToTable("SEC_Identity");
            builder.Entity<Role>().ToTable("SEC_Role");
            builder.Entity<IdentityClaim>().ToTable("SEC_IdentityClaim");
            builder.Entity<IdentityRole>().ToTable("SEC_IdentityRole");
            builder.Entity<IdentityLogin>().ToTable("SEC_IdentityLogin");
            builder.Entity<RoleClaim>().ToTable("SEC_RoleClaim");
            builder.Entity<IdentityToken>().ToTable("SEC_IdentityToken");
        }

    }
}
