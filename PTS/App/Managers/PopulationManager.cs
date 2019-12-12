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
        public const int MAX_NUMBER_JOURNEY = 300;

        public PopulationManager(MySqlConnection dbConn, Dictionary<string, string> cities)
        {
            this.dbConn = dbConn;
            this.journeyManager = new JourneyManager(dbConn, cities);

            //The number of journey = 15% of all possibilities
            NUMBER_JOURNEY = (int)(Utils.Utils.Factor(cities.Count - 1) * 0.15);
            //If number of journey > 100 set it to 100
            NUMBER_JOURNEY = NUMBER_JOURNEY > MAX_NUMBER_JOURNEY ? MAX_NUMBER_JOURNEY : NUMBER_JOURNEY;
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
                    tempJourney.m
                }

                //Else add it to the list
                journeys.Add(new Journey(tempJourney));
            }

            //Create the population
            Population population = new Population(journeys);

            return population;
            //throw new NotImplementedException();
        }

        public Population NextGen(Population population, Func<List<Journey>, Journey> selectionMethode)
        {   
            //New list of journey
            List<Journey> journeys = new List<Journey>();

            Journey parent1,
                    parent2,
                    child;

            for (int i = 0; i < NUMBER_JOURNEY; i++)
            {
                do
                {
                    //First step : get two parents  
                    parent1 = selectionMethode(population.Journeys);
                    parent2 = selectionMethode(population.Journeys);

                    //Second step : crossing method
                    child = parent1.CrossoverWith(parent2);

                } while (journeys.Exists(j => j != null && j.Cities.SequenceEqual(child.Cities)));

                //Third step : Add the child to the list
                journeys.Add(new Journey(child));
            }

            return new Population(journeys);           
        }
        
    }
}