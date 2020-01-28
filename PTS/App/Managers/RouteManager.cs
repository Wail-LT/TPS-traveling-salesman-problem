using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using PTS.App.Objects;
using PTS.App.Utils;

namespace PTS.App.Managers
{
    public class RouteManager
    {
        private List<City> cities;
        private CityManager cityManager;

        public RouteManager(MySql.Data.MySqlClient.MySqlConnection dbConn, Dictionary<string, string> cities)
        {
            //Init the cityManager
            cityManager = new CityManager(dbConn);

            //Get the list of cities
            List<City> citiesTemp = cityManager.GetCities(cities);

            this.cities = new List<City>(citiesTemp);

        }

        public Route NextRoute()
        {
            //Randomize the cities
            cities.Shuffle(1);

            return new Route(new List<City>(cities));
        }
    }
}
