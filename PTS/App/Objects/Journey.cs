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

        public List<City> Cities => cities;

        public Journey(List<City> cities)
        {
            this.cities = new List<City>(cities);
            ComputeFitness();
        }
        
       /*
        * Compare 2 journeys using their fitness
        * returns : 0 if equals;
        *           -1 if this is better;
        *           1 if other is better;
        */
        public int CompareTo([AllowNull] Journey other)
        {
            if (other == null || this.Fitness < other.Fitness)
                return -1;

            if (this.Fitness > other.Fitness)
                return 1;

            return 0;
        }

        public Journey CrossoverWith( Journey other)
        {
            List<City> cities1 = this.Cities;
            List<City> cities2 = other.Cities;

            Random rand = Utils.Utils.Random;

            int pivot1 = rand.Next(1, cities1.Count - 2);
            int pivot2 = rand.Next(1, cities1.Count - 2);

            while (pivot2 == pivot1)
            {
                pivot2 = rand.Next(1, cities1.Count - 2);
            }

            if (pivot1 > pivot2)
            {
                int tmp = pivot1;
                pivot1 = pivot2;
                pivot2 = tmp;
            }

            List<City> child = new List<City>(cities1);

            for (int i = 0; i < cities1.Count; i++)
            {
                if (i >= pivot1 && i <= pivot2)
                    child[i] = cities2[i];
            }

            foreach (City city in cities1)
            {
                if (!child.Contains(city))
                {
                    var grp = child.GroupBy(c => c);
                    foreach (var g in grp)
                    {
                        if (g.Count() > 1)
                            child[child.IndexOf(g.Key)] = city;
                    }
                }
            }

            return new Journey(child);
        }

        public string ToString()
        {
            string str = "Journey : ";
            foreach (City c in cities)
                str += c.ToString() + ", ";
            str += " Fitness : " + fitness + "\n";

            return str;
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
