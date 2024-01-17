using AzurLaneBBot.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzurLaneBBot.Database
{
    public interface IDatabaseService
    {
        public IEnumerable<BoobaBotProject?> GetAllBBPShips();

        public BoobaBotProject? GetBBPShip(string name);
    }
}
