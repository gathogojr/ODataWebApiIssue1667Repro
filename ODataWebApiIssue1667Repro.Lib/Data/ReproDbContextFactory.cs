using Microsoft.Extensions.Configuration;
using System.Data.Entity.Infrastructure;
using System.IO;

namespace Repro.Lib.Data
{
    public class ReproDbContextFactory : IDbContextFactory<ReproDbContext>
    {
        public ReproDbContext Create()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(@Directory.GetCurrentDirectory() + "/../../../ODataWebApiIssue1667Repro/appsettings.json").Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            return new ReproDbContext(connectionString);

            // Enable-Migrations
            // Update-Database -TargetMigration:0
            // Add-Migration InitialMigration -Force
            // Update-Database
        }
    }
}
