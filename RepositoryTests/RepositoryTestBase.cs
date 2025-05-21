using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace RepositoryTests;

public abstract class RepositoryTestBase : IDisposable {
    protected readonly AzurLaneBBotDbContext DbContext;

    protected RepositoryTestBase() {
        var options = new DbContextOptionsBuilder<AzurLaneBBotDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        DbContext = new AzurLaneBBotDbContext(options);
    }

    public void Dispose() {
        DbContext.Dispose();
        GC.SuppressFinalize(this); // Fix for CA1816
    }
}