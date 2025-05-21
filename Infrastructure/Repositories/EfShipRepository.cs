using Domain.Interfaces;
using Domain.ShipAggregate;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class EfShipRepository(IApplicationDbContext context) : GenericRepository<Ship>(context), IShipRepository {

    public async Task<Ship?> GetByNameAsync(string name) {
        return await _context.Ships
            .FirstOrDefaultAsync(s => s.Name.ToLower().Equals(name.ToLower()));
    }
}