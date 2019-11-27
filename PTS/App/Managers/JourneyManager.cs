using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using PTS.App.Objects;
using PTS.App.Utils;

namespace PTS.App.Managers
{
    public class JourneyManager
    {
        private List<City> cities;
        private CityManager cityManager;

        public JourneyManager(MySql.Data.MySqlClient.MySqlConnection dbConn, Dictionary<string, string> cities)
        {
            //Init the cityManager
            cityManager = new CityManager(dbConn);

            //Get the list of cities
            List<City> citiesTemp = cityManager.GetCities(cities);

            this.cities = new List<City>(citiesTemp);
        }

        public Journey NextJourney()
        {
            //Randomize the cities
            cities.Shuffle();

            return new Journey(new List<City>(cities));
        }

        public Journey Crossover(Journey parent1, Journey parent2)
        {
            List<City> cities1 = parent1.Cities;
            List<City> cities2 = parent2.Cities;

            Random rand = new Random();
            int pivot1 = rand.Next(1, cities1.Count - 2);
            int pivot2 = rand.Next(1, cities1.Count - 2);
            
            while (pivot2 == pivot1)
            {
                pivot2 = rand.Next(1, cities1.Count - 2);
            }

            if(pivot1 > pivot2)
            {
                int tmp = pivot1;
                pivot1 = pivot2;
                pivot2 = tmp;
            }

            List<City> child = new List<City>(cities1);

            for (int i=0; i<cities1.Count; i++)
            {
                if (i >= pivot1 && i <= pivot2)
                    child[i] = cities2[i];
            }

            for (int i = 0; i < cities1.Count; i++)
            {
                if (i >= pivot1 && i <= pivot2)
                {
                    child[i] = cities2[i];
                }
            }

            foreach(City city in cities1)
            {
                if (!child.Find(city))
                {
                    var grp = child.GroupBy(c => c);
                    foreach(var g in grp)
                    {
                        if (g.Count() > 1)
                            child[child.IndexOf(g.Key)] = city;
                    }
                }
            }

            return new Journey(child);
        }
    }
}
