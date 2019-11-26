using System;
using System.Collections.Generic;
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

    }
}
