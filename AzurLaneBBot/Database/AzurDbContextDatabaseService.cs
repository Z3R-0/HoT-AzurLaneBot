using AzurLaneBBot.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzurLaneBBot.Database {
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
            if(name == null) throw new ArgumentNullException(nameof(name));

            return _dbContext.BoobaBotProjects.Where(b => b.Name.ToLower() == name.ToLower().Trim()).FirstOrDefault();
        }

        public IEnumerable<BoobaBotProject> GetAllBBPShips() {
            return _dbContext.BoobaBotProjects;
        }
    }
}
