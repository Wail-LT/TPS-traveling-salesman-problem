using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTS.App.Objects
{
    public class Journey
    {
        List<City> cities;

        public Journey(List<City> cities)
        {
            this.cities = new List<City>(cities);
        }

        public double GetFitness()
        {
            double fitness = 0;
            for (int i = 0; i < cities.Count - 1; i++) 
            {
                fitness += cities[i].GetDistanceTo(cities[i + 1]);
            }

            return fitness;
        }

    }
}
