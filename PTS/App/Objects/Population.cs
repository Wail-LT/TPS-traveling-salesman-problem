using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTS.App.Objects
{
    public class Population
    {
        private List<Journey> journeys;

        /*Properties*/
        public double BestFitness => this.journeys.First().Fitness;
        public List<Journey> Journeys => journeys;

        public Population(List<Journey> journeys)
        {
            //Copy the list of journeys
            this.journeys = new List<Journey>(journeys);

            //Sort the list
            this.journeys.Sort();
        }

    }
}
