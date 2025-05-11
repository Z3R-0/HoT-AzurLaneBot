using Domain.Interfaces;
using Domain.ShipAggregate;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class EfShipRepository(IApplicationDbContext context) : GenericRepository<Ship>(context), IShipRepository {

    public async Task<Ship?> GetByNameAsync(string name) {
        return await _context.Ships
            .Include(s => s.Skins)
            .FirstOrDefaultAsync(s => s.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
    }

    public async Task<IEnumerable<Ship>> GetAllAsync(bool includeSkins = true) {
        IQueryable<Ship> query = includeSkins
        ? _context.Ships.Include(s => s.Skins)
        : _context.Ships;

        return await query.ToListAsync();
    }
}