using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using PTS.App.Objects;

namespace PTS.App.Managers
{
    public class PopulationManager
    {
        private MySqlConnection dbConn;
        private RouteManager routeManager;

        public readonly int NUMBER_ROUTE;
        public const int MAX_NUMBER_ROUTE = 300;

        public PopulationManager(MySqlConnection dbConn, Dictionary<string, string> cities)
        {
            this.dbConn = dbConn;
            this.routeManager = new RouteManager(dbConn, cities);

            //The number of route = 15% of all possibilities
            NUMBER_ROUTE = (int)(Utils.Utils.Factor(cities.Count - 1) * 0.15);
            //If number of route > 100 set it to 100
            NUMBER_ROUTE = NUMBER_ROUTE > MAX_NUMBER_ROUTE ? MAX_NUMBER_ROUTE : NUMBER_ROUTE;
        }

        public Population GeneratePopulation()
        {
            //New list of route
            List<Route> routes = new List<Route>();

            Route tempRoute;

            //Create routes to create the population
            for (int i = 0; i < NUMBER_ROUTE; i++)
            {
                tempRoute = routeManager.NextRoute();

                //If the route already exists in the list generate another one
                while (routes.Exists(j => j != null && j.Cities.SequenceEqual(tempRoute.Cities)))
                {
                    tempRoute = routeManager.NextRoute();
                }

                //Else add it to the list
                routes.Add(new Route(tempRoute));
            }

            //Create the population
            Population population = new Population(routes);

            return population;
            //throw new NotImplementedException();
        }

        //Method "BEFORE"
        /*public static Route PreSelect(List<City> cities)
        {
            List<double> totalDistances = new List<double>(cities.Count);
            List<City> parentRoute = new List<City>(cities.Count);
            List<City> adjacentCities = new List<City>(cities);

            //Calculate total sum of distances from each city to the others
            for (int i = 0; i < adjacentCities.Count; i++)
            {
                double dist = 0;
                for (int j = 0; j < adjacentCities.Count; j++)
                {
                    if (i != j)
                        dist += adjacentCities[i].GetDistanceTo(adjacentCities[j]);
                }
                totalDistances.Add(dist);
            }

            double minDist = totalDistances.Min();
            City startCity = adjacentCities[totalDistances.IndexOf(minDist)];
            City lastCity = startCity;
            parentRoute.Add(startCity);
            adjacentCities.Remove(startCity);

            List<double> delta = new List<double>(adjacentCities.Count);
            for (int i = 0; i < adjacentCities.Count; i++)
            {
                City nextCity = Route.CompareCities(startCity, lastCity, adjacentCities);
                parentRoute.Add(nextCity);
                adjacentCities.Remove(nextCity);
                lastCity = nextCity;
            }
            parentRoute.Add(adjacentCities[0]);

            //Console.WriteLine(string.Join(",", adjacentCities));

            return new Route(parentRoute);

        }*/

        public Population NextGen(Population population, Func<List<Route>, Route> selectionMethode, double mutateFactor)
        {   
            //New list of route
            List<Route> routes = new List<Route>();

            Route parent1,
                  parent2,
                  child;

            for (int i = 0; i < NUMBER_ROUTE; i++)
            {
                do
                {
                    //First step : get two parents  
                    parent1 = selectionMethode(population.Routes);
                    parent2 = selectionMethode(population.Routes);

                    //Second step : crossing method
                    child = parent1.CrossoverWith(parent2);
                    
                } while (routes.Exists(j => j != null && j.Cities.SequenceEqual(child.Cities)));

                if (Utils.Utils.Random.NextDouble() < mutateFactor)
                    child.Mutate();

                //Third step : Add the child to the list
                routes.Add(new Route(child));
            }

            return new Population(routes);           
        }
        
    }
}