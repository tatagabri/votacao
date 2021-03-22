using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Votacao.Repository;

namespace Votacao
{
    public class ModelContextFactory : IDbContextFactory<ModelContext>
    {
        private string ConnectionString { get; set; }

        public ModelContextFactory()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            ConnectionString = configuration.GetConnectionString(Startup.DEFAULT_CONNECTION_STRING);
        }

        public ModelContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ModelContext>();
            optionsBuilder.UseSqlServer(ConnectionString);
            return new ModelContext(optionsBuilder.Options);
        }
    }
}
