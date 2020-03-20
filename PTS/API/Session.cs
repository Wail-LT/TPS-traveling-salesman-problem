using PTS.App.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTS.API
{
    public class Session
    {
        private PopulationManager populationManager;
        private DateTime lastUse;
        public DateTime LastUse { get => lastUse; set => lastUse = value; }

        public Session()
        {
            lastUse = DateTime.UtcNow;
        }
        public Session(Dictionary<string, string> cities)
        {
            populationManager = new PopulationManager(cities);
            lastUse = DateTime.UtcNow;
        }
        public PopulationManager GetPopulationManager()
        {
            return populationManager;
        }
        public void SetPopulationManager(Dictionary<string, string> cities)
        {
            populationManager = new PopulationManager(cities);
        }
    }
}
