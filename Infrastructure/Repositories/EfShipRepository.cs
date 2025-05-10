using Domain.Interfaces;
using Domain.ShipAggregate;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class EfShipRepository(AzurLaneBBotDbContext context) : IShipRepository {
    private readonly AzurLaneBBotDbContext _context = context;

    public async Task<Ship?> GetByIdAsync(Guid id) {
        return await _context.Ships
            .Include(s => s.Skins)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

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

    public async Task AddAsync(Ship ship) {
        await _context.Ships.AddAsync(ship);
    }

    public async Task UpdateAsync(Ship ship) {
        await _context.Ships.AddAsync(ship);
    }

    public async Task DeleteAsync(Guid id) {
        var ship = await _context.Ships.FindAsync(id);
        if (ship is not null) {
            _context.Ships.Remove(ship);
        }
    }
}