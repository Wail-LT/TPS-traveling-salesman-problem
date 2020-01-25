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
        public readonly List<City> cities;
        private double fitness;
        
        public double Fitness => fitness;

        public Route(List<City> cities)
        {
            this.cities = new List<City>(cities);
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
            List<City> cities1 = this.cities;
            List<City> cities2 = other.cities;

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

            int n = cities.Count;
            double mutateFactor = 1 / 3;

            while (n > 1)
            {
                n--;
                if (random.NextDouble() < mutateFactor)
                {
                    int k = random.Next(1, n + 1);
                    City tempCity = cities[k];
                    cities[k] = cities[n];
                    cities[n] = tempCity;
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
            for (int i = 0; i < cities.Count - 1; i++)
            {
                this.fitness += cities[i].GetDistanceTo(cities[i + 1]);
            }
            this.fitness += cities.Last().GetDistanceTo(cities.First());

            //this.fitness = Math.Round(this.fitness / 1000, 2);
        }

        public static double Delta(List<double> val1, List<double> val2)
        {
            return val1[0] - val2[0] + val1[1] - val2[1] + val1[3] - val2[2];
        }
        
        public static City CompareCities(City refCity, City lastCity, List<City> others)
        {
            Dictionary<City, List<double>> infos = new Dictionary<City, List<double>>();

            City bestCity;
            if (others.Count > 2)
            {
                //Gathering data
                for (int i = 0; i < others.Count; i++)
                {
                    double distToRef = others[i].GetDistanceTo(refCity);
                    double nearest = 0;
                    double furthest = 0;


                    for (int j = 0; j < others.Count; j++)
                    {
                        if (i != j)
                        {
                            double distToNeighbor = others[i].GetDistanceTo(others[j]);
                            if (distToNeighbor < nearest)
                                nearest = distToNeighbor;
                            else if (distToNeighbor > furthest)
                                furthest = distToNeighbor;
                        }
                    }
                    infos.Add(others[i], new List<double>() { distToRef, nearest, furthest });
                }

                //Comparing cities
                double delta = 0;
                List<City> citiesToCompare = new List<City>(others);
                bestCity = citiesToCompare[0];

                for (int i = 1; i < citiesToCompare.Count; i++)
                {
                    delta = Delta(infos[bestCity], infos[citiesToCompare[i]]);
                    if (delta > 0)
                        bestCity = citiesToCompare[i];
                    else if (delta == 0)
                    {
                        double dist1 = bestCity.GetDistanceTo(refCity);
                        double dist2 = citiesToCompare[i].GetDistanceTo(refCity);
                        if (dist1 > dist2)
                            bestCity = citiesToCompare[i];
                    }
                }
            }
            else if (others.Count == 2)
            {
                //For the last 2 cities
                double delta = others[0].GetDistanceTo(refCity) - others[1].GetDistanceTo(refCity) +
                    others[0].GetDistanceTo(lastCity) - others[1].GetDistanceTo(lastCity);
                if (delta > 0)  //this time we take the biggest value
                    bestCity = others[0];
                else bestCity = others[1];
            }
            else bestCity = others[0];
            
            return bestCity;

        }

    }
}
