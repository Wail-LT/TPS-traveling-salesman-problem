using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using PTS.App.Utils;
using PTS.App.Objects;

namespace PTS.App.Managers
{
    public class PopulationManager
    {
        private readonly RouteManager routeManager;

        public readonly int NUMBER_ROUTE;
        public const int MAX_NUMBER_ROUTE = 300;

        public PopulationManager (Dictionary<string, string> cities)
        {
            this.routeManager = new RouteManager(cities);
            //The number of route = 15% of all possibilities
            ulong number = (ulong)(Utils.Utils.Factor(cities.Count - 1) * 0.15);
            //If number of route > 100 set it to 100
            NUMBER_ROUTE = number > MAX_NUMBER_ROUTE ? MAX_NUMBER_ROUTE : (int)number;
        }

        public Population GeneratePopulation(Func<List<City>, int, Population> iniFunc = null)
        {
            if (iniFunc != null)
                return iniFunc(routeManager.cities, NUMBER_ROUTE);

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
                    
                } while (routes.Exists(route => route != null && route.Cities.SequenceEqual(child.Cities)));

                if (Utils.Utils.Random.NextDouble() < mutateFactor)
                    child.Mutate(mutateFactor);

                //Third step : Add the child to the list
                routes.Add(new Route(child));
            }

            return new Population(routes);           
        }
    }
}