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
            this.cities = cities;
        }
        public List<City> Cities => cities;

        public double GetFitness()
        {
            return 0;
        }

        private double GetDistance()
        {
            double dist = 0;
            for(int i=0; i<cities.Count-1; i++)
            {
                dist += cities[i].GetDistance(cities[i + 1]);
            }
            return dist;
        }

    }
}
