using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace PTS.App.Objects
{
    public class Journey : IComparable<Journey>
    {
        private List<City> cities;
        private double fitness;

        public double Fitness => fitness;


        public Journey(List<City> cities)
        {
            this.cities = new List<City>(cities);
            ComputeFitness();
        }

       /*
        * Compare 2 journeys using their fitness
        * returns : 0 if equals;
        *           1 if this is better;
        *           -1 if other is better;
        */
        public int CompareTo([AllowNull] Journey other)
        {
            if (other == null || this.Fitness < other.Fitness)
                return 1;

            if (this.Fitness > other.Fitness)
                return -1;

            return 0;
        }


        private void ComputeFitness()
        {
            this.fitness = 0;
            for (int i = 0; i < cities.Count - 1; i++)
            {
                this.fitness += cities[i].GetDistanceTo(cities[i + 1]);
            }
        }

    }
}
