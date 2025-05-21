using Domain.Interfaces;
using Infrastructure.Data;

namespace Infrastructure;
public class EfUnitOfWork(AzurLaneBBotDbContext context) : IUnitOfWork {
    private readonly AzurLaneBBotDbContext _context = context;

    public Task SaveChangesAsync(CancellationToken cancellationToken = default) {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
