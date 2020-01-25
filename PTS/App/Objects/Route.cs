using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace PTS.App.Objects
{
    public class Route : IComparable<Route>
    {
        //Initilized only in constructor so use readonly is a better practice 
        public readonly List<City> Cities;
        private double fitness;
        
        public double Fitness => fitness;

        public Route(List<City> cities)
        {
            this.Cities = new List<City>(cities);
            ComputeFitness();
        }

        public Route(Route route)
        {
            this.Cities = new List<City>(route.Cities);
            this.fitness = route.fitness;
        }

        /*
         * Compare 2 routes using their fitness
         * returns : 0 if equals;
         *           -1 if this is better;
         *           1 if other is better;
         */
        public int CompareTo([AllowNull] Route other)
        {
            if (other == null || this.Fitness < other.Fitness)
                return -1;

            if (other != null && this.Fitness > other.Fitness)
                return 1;

            return 0;
        }

        public Route CrossoverWith( Route other)
        {
            List<City> cities1 = this.Cities;
            List<City> cities2 = other.Cities;

            Random rand = Utils.Utils.Random;

            int pivot1 = rand.Next(1, cities1.Count - 1);
            int pivot2 = rand.Next(1, cities1.Count - 1);

            while (pivot2 == pivot1)
            {
                pivot2 = rand.Next(1, cities1.Count - 1);
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

            return new Route(child);
        }

        //fonction mutate qui donne la chance à chaque ville du trajet de s'échanger avec une autre ville
        public void Mutate()
        {
            Random random = Utils.Utils.Random;

            int n = Cities.Count;
            double mutateFactor = 1 / 3;

            while (n > 1)
            {
                n--;
                if (random.NextDouble() < mutateFactor)
                {
                    int k = random.Next(1, n + 1);
                    City tempCity = Cities[k];
                    Cities[k] = Cities[n];
                    Cities[n] = tempCity;
                }

            }
        }



        /*Override*/
        public override string ToString()
        {
            string str = "Route : ";
            foreach (City c in Cities)
                str += c.ToString() + ", ";
            str += " Fitness : " + Math.Round(this.fitness / 1000, 2) + "  " + fitness + " km\n";

            return str;
        }



        /*Private*/
        private void ComputeFitness()
        {
            this.fitness = 0;
            for (int i = 0; i < Cities.Count - 1; i++)
            {
                this.fitness += Cities[i].GetDistanceTo(Cities[i + 1]);
            }
            this.fitness += Cities.Last().GetDistanceTo(Cities.First());

            //this.fitness = Math.Round(this.fitness / 1000, 2);
        }

    }
}
