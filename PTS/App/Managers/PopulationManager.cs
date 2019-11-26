﻿using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using PTS.App.Objects;

namespace PTS.App.Managers
{
    public class PopulationManager
    {
        private MySqlConnection dbConn;
        private JourneyManager journeyManager;

        public const int NUMBER_JOURNEY = 100;

        public PopulationManager(MySqlConnection dbConn, Dictionary<string, string> cities)
        {
            this.dbConn = dbConn;
            this.journeyManager = new JourneyManager(dbConn, cities);
        }

        public Population GeneratePopulation()
        {
            //New list of journey
            List<Journey> journeys = new List<Journey>();

            //Create journeys to create the population
            for (int i = 0; i < NUMBER_JOURNEY; i++)
            {
                journeys.Add(journeyManager.NextJourney());
            }

            //Create the population
            Population population = new Population(journeys);

            return population;
            //throw new NotImplementedException();
        }

        public Population NextGen(Population population)
        {
            throw new NotImplementedException();
        }
    }
}
