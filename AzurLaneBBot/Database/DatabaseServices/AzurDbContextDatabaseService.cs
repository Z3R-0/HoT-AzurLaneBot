using AzurLaneBBot.Database.Models;

namespace AzurLaneBBot.Database.DatabaseServices {
    public class AzurDbContextDatabaseService : IDatabaseService {
        private AzurlanedbContext _dbContext;

        public AzurDbContextDatabaseService(AzurlanedbContext azurlanedbContext) {
            _dbContext = azurlanedbContext;
        }

        // Empty constructor for mocking
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public AzurDbContextDatabaseService() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public BoobaBotProject? GetBBPShip(string name) {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            return _dbContext.BoobaBotProjects.Where(b => b.Name.Equals(name.ToLower(), StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
        }

        public IEnumerable<BoobaBotProject> GetAllBBPShips() {
            return _dbContext.BoobaBotProjects;
        }

        public bool UpdateBBShipImageURL(string shipToUpdateName, string imageUrl) {
            using (var dbContext = _dbContext) {
                var shipEntry = dbContext.BoobaBotProjects.Where(b => b.Name.Equals(shipToUpdateName.ToLower(), StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

                if (shipEntry != null) {
                    shipEntry.ImageUrl = imageUrl;

                    try {
                        dbContext.SaveChanges();
                        // Successfully saved entry
                        return true;
                    } catch {
                        // Could not save changes
                        return false;
                    }
                }

                return false;
            }
        }
    }
}
