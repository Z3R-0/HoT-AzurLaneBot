using Domain;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class GenericRepository<T>(IApplicationDbContext context) : IGenericRepository<T> where T : class, IEntity {
    protected readonly IApplicationDbContext _context = context;
    protected readonly DbSet<T> _dbSet = context.Set<T>();

    public virtual async Task<T?> GetByIdAsync(Guid id) {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync() {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task AddAsync(T entity) {
        await _dbSet.AddAsync(entity);
    }

    public virtual Task UpdateAsync(T entity) {
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public virtual async Task DeleteAsync(Guid id) {
        var entity = await GetByIdAsync(id);
        if (entity != null) {
            _dbSet.Remove(entity);
        }
    }
}