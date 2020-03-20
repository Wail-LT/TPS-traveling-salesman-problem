using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTS.App.Objects
{
    public class Population
    {
        private List<Route> routes;

        /*Properties*/
        public Route BestRoute => this.routes.First();
        public double BestFitness => this.routes.First().Fitness;
        public List<Route> Routes => routes;

        public Population(List<Route> routes)
        {
            //Copy the list of routes
            this.routes = new List<Route>(routes);

            //Sort the list
            this.routes.Sort();
        }

        public override string ToString()
        {
            string str =  "Population : \n";
            foreach (Route j in routes)
                str += j.ToString();

            return str;
        }

    }
}
