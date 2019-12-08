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
        private JourneyManager journeyManager;

        public readonly int NUMBER_JOURNEY;

        public PopulationManager(MySqlConnection dbConn, Dictionary<string, string> cities)
        {
            this.dbConn = dbConn;
            this.journeyManager = new JourneyManager(dbConn, cities);

            //The number of journey = 15% of all possibilities
            NUMBER_JOURNEY = (int)(Utils.Utils.Factor(cities.Count - 1) * 0.15);
        }

        public Population GeneratePopulation()
        {
            //New list of journey
            List<Journey> journeys = new List<Journey>();

            Journey tempJourney;

            //Create journeys to create the population
            for (int i = 0; i < NUMBER_JOURNEY; i++)
            {
                tempJourney = journeyManager.NextJourney();

                //If the journey already exists in the list generate another one
                while (journeys.Exists(j => j != null && j.Cities.SequenceEqual(tempJourney.Cities)))
                {
                    tempJourney = journeyManager.NextJourney();
                }

                //Else add it to the list
                journeys.Add(new Journey(tempJourney));
            }

            //Create the population
            Population population = new Population(journeys);

            return population;
            //throw new NotImplementedException();
        }

        public Population NextGen(Population population, Func<List<Journey>, Journey> mafunction)
        {   
            //New list of journey
            List<Journey> journeys = new List<Journey>();

            for (int i = 0; i < NUMBER_JOURNEY; i++)
            {
                //First step : get two parents  
                Journey parent1 = mafunction(population.Journeys);
                Journey parent2 = mafunction(population.Journeys);

                //Second step : crossing method
                Journey child = parent1.CrossoverWith(parent2);

                //Third step : Add the child to the list
                journeys.Add(child);
            }

            return new Population(journeys);           
        }
        
    }
}
