using System;
using PTS.App.Managers;
using PTS.App.Objects;
using PTS.App.DataBase;
using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Collections.Generic;

namespace PTS.App
{
    public class App
    {
        //Managers
        private PopulationManager populationManager;

        //App components
        private Population population;
        
        private App(MySqlConnection dbConn, List<string> cities)
        {
            //1. init the populationManager with connection to the database
            populationManager = new PopulationManager(dbConn);

            //2. Generate the first population
            population = populationManager.GeneratePopulation(cities);
        }

        private void NextGen()
        {
            //Generate the next generation
            Population nextPopulation = populationManager.NextGen(population);

            //store it in the population variable
            population = nextPopulation;
        }

        private Population GetPopulation()
        {
            return population;
        }

        public static void Start(int nbGeneration)
        {
            //Setup the database connection
            try
            {
                DataBaseManager.SetupConnection();
            }
            catch(Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }

            if (DataBaseManager.connection != null)
            {
                //init the app with database connection
                //App app = new App(DataBaseManager.connection);

                MySqlCommand rqst = DataBaseManager.connection.CreateCommand();

                rqst.CommandText = "SELECT * FROM `villes_france_free` where ville_nom_simple = 'paris'";
                
                using (DbDataReader reader = rqst.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int ville = reader.GetOrdinal("ville_nom_simple");
                            Console.WriteLine("Ville : {0}", reader.GetString(ville));
                            Console.WriteLine("Longitude : {0}", reader.GetOrdinal("ville_longitude_deg"));
                            Console.WriteLine("Latitude : {0}", reader.GetOrdinal("ville_latitude_deg"));
                        }
                    }
                }

                /*double bestFitness = app.GetPopulation().GetBestFitness();

                for (int i = 0; i < nbGeneration; i++)
                {
                    //Get and print the best fitness of the generation and the generation number
                    Console.WriteLine("Generation {0}: {1};", i, app.GetPopulation().GetBestFitness());

                    //Store the fitness if it's better than the previous one
                    if (bestFitness > app.GetPopulation().GetBestFitness())
                        bestFitness = app.GetPopulation().GetBestFitness();

                    //Generate the next generation
                    app.NextGen();
                }*/
            }

        }
    }
}
