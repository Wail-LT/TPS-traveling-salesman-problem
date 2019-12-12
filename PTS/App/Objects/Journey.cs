using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace PTS.App.Objects
{
    public class Journey : IComparable<Journey>
    {
        //Initilized only in constructor so use readonly is a better practice 
        public readonly List<City> Cities;
        private double fitness;
        
        public double Fitness => fitness;

        public Journey(List<City> cities)
        {
            this.Cities = new List<City>(cities);
            ComputeFitness();
        }

        public Journey(Journey journey)
        {
            this.Cities = new List<City>(journey.Cities);
            this.fitness = journey.fitness;
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

            if (other != null && this.Fitness > other.Fitness)
                return 1;

            return 0;
        }

        public Journey CrossoverWith( Journey other)
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

            return new Journey(child);
        }

        //fonction mutate qui donne la chance à chaque ville du trajet de s'échanger avec une autre ville
        public void mutate(double prob_mut)
        {
            Random random = Utils.Utils.Random;
            int indexPos2;
            for (int i = 0; i < this.Cities.Count; i++)
            {
                if (random.NextDouble() < prob_mut)
                {
                    indexPos2 = random.Next(0, Cities.Count);
                    City ville1 = Cities[i];
                    City ville2 = Cities[indexPos2];

                    Cities[i] = ville2;
                    Cities[indexPos2] = ville1;
                }


            }
        }



        /*Override*/
        public override string ToString()
        {
            string str = "Journey : ";
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
