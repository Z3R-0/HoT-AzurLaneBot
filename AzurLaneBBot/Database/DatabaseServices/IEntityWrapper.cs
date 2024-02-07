namespace AzurLaneBBot.Database.DatabaseServices {
    public interface IEntityEntryWrapper<T> where T : class {
        T Add(T entity);

        public T Update(T originalEntity, T updatedEntity);
    }
}
