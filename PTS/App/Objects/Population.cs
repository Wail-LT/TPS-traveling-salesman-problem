using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTS.App.Objects
{
    public class Population
    {
        private List<Route> route;

        /*Properties*/
        public Route BestRoute => this.route.First();
        public double BestFitness => this.route.First().Fitness;
        public List<Route> Routes => route;

        public Population(List<Route> routes)
        {
            //Copy the list of routes
            this.route = new List<Route>(routes);

            //Sort the list
            this.route.Sort();
        }

        public override string ToString()
        {
            string str =  "Population : \n";
            foreach (Route j in route)
                str += j.ToString();

            return str;
        }

    }
}
