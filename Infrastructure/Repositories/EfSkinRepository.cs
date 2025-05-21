using Domain.Interfaces;
using Domain.SkinAggregate;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class EfSkinRepository(IApplicationDbContext context) : GenericRepository<Skin>(context), ISkinRepository {
    public async Task<Skin?> GetByNameAsync(string name) {
        return await _context.Skins
            .FirstOrDefaultAsync(s => s.Name.ToLower().Equals(name.ToLower()));
    }
}
