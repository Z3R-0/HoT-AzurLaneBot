using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data;
public class AzurLaneBBotDbContextFactory : IDesignTimeDbContextFactory<AzurLaneBBotDbContext> {
    public AzurLaneBBotDbContext CreateDbContext(string[] args) {
        // Build configuration from appsettings.json
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = config.GetConnectionString("Default");

        var optionsBuilder = new DbContextOptionsBuilder<AzurLaneBBotDbContext>();
        optionsBuilder.UseSqlite(connectionString);

        return new AzurLaneBBotDbContext(optionsBuilder.Options);
    }
}
