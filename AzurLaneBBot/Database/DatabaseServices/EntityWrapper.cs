using Microsoft.EntityFrameworkCore;

namespace AzurLaneBBot.Database.DatabaseServices {
    public class EntityEntryWrapper<T> : IEntityEntryWrapper<T> where T : class {
        private readonly DbContext _context;

        public EntityEntryWrapper(DbContext context) {
            _context = context;
        }

        public T Add(T entity) {
            var entry = _context.Entry(entity);
            entry.State = EntityState.Added;
            return entity;
        }

        public T Update(T originalEntity, T updatedEntity) {
            var originalEntry = _context.Entry(originalEntity);
            originalEntry.CurrentValues.SetValues(updatedEntity);
            originalEntry.State = EntityState.Modified;
            return originalEntity;
        }
    }
}
