using AzurLaneBBot.Database.DatabaseServices;
using AzurLaneBBot.Database.Models;

namespace AzurLaneBBot.Modules.Commands {
    public class ManagementCommandHandlers {
        protected IDatabaseService _dbService;

        public ManagementCommandHandlers(AzurlanedbContext azurlanedbContext) {
            _dbService = new AzurDbContextDatabaseService(azurlanedbContext);
        }
    }
}
