using AzurLaneBBot.Database.Models;

namespace AzurLaneBBot.Database.DatabaseServices {
    public class AzurDbContextDatabaseService : IDatabaseService {
        private readonly AzurlanedbContext _dbContext;
        private readonly IEntityEntryWrapper<BoobaBotProject> _entityEntryWrapper;

        public AzurDbContextDatabaseService(AzurlanedbContext azurlanedbContext, IEntityEntryWrapper<BoobaBotProject> entityEntryWrapper) {
            _dbContext = azurlanedbContext;
            _entityEntryWrapper = entityEntryWrapper;
        }

        public AzurDbContextDatabaseService(AzurlanedbContext azurlanedbContext) {
            _dbContext = azurlanedbContext;
            _entityEntryWrapper = new EntityEntryWrapper<BoobaBotProject>(_dbContext);
        }

        // Empty constructor for mocking
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public AzurDbContextDatabaseService() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public BoobaBotProject? GetBBPShip(string name) {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            return _dbContext.BoobaBotProjects.Where(b => b.Name.ToLower().Equals(name.ToLower())).FirstOrDefault();
        }

        public IEnumerable<BoobaBotProject> GetAllBBPShips() {
            return _dbContext.BoobaBotProjects;
        }

        public bool UpdateBBShipImageURL(string shipToUpdateName, string imageUrl) {
            var shipEntry = _dbContext.BoobaBotProjects.Where(b => b.Name.ToLower().Equals(shipToUpdateName.ToLower())).FirstOrDefault();

            if (shipEntry != null) {
                shipEntry.ImageUrl = imageUrl;

                try {
                    _dbContext.SaveChanges();
                    // Successfully saved entry
                    return true;
                } catch {
                    // Could not save changes
                    return false;
                }
            }

            return false;
        }

        public BoobaBotProject AddBBShip(BoobaBotProject shipToCreate) {
            var newEntry = _entityEntryWrapper.Add(shipToCreate);

            _dbContext.SaveChanges();

            return newEntry;
        }

        public BoobaBotProject UpdateBBShip(BoobaBotProject updatedShip) {
            var originalEntry = GetBBPShip(updatedShip.Name);

            var updatedEntry = _entityEntryWrapper.Update(originalEntry, updatedShip);

            _dbContext.SaveChanges();

            return updatedEntry;
        }

        public bool DeleteBBShip(string shipToDeleteName) {
            var toDeleteEntry = GetBBPShip(shipToDeleteName);

            if (toDeleteEntry == null)
                throw new ArgumentException("Provided ship name did not exist in database", nameof(shipToDeleteName));

            _dbContext.Remove(toDeleteEntry);

            _dbContext.SaveChanges();

            return true;
        }
    }
}
