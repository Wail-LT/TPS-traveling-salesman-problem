using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTS.App.Objects
{
    public class Population
    {
        private List<Journey> journeys;

        public Population(List<Journey> journeys)
        {
            this.journeys = new List<Journey>(journeys);
        }

        public double GetBestFitness()
        {
            return 0;
        }




    }
}
