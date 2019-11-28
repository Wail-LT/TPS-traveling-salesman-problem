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
        
        private App(MySqlConnection dbConn, Dictionary<string, string> cities)
        {
            //1. init the populationManager with connection to the database
            populationManager = new PopulationManager(dbConn, cities);

            //2. Generate the first population
            population = populationManager.GeneratePopulation();
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

                Dictionary<string, string> cities = new Dictionary<string, string>();

                cities.Add("34000", "Montpellier");
                cities.Add("38100", "Grenoble");
                cities.Add("75001", "Paris");
                cities.Add("44100", "Nantes");
                cities.Add("68100", "Mulhouse");
                cities.Add("59300", "Valenciennes");

                //init the app with database connection
                App app = new App(DataBaseManager.connection, cities);

                double bestFitness = app.population.BestFitness;

                for (int i = 0; i < 99; i++)
                {
                    //Print the current population
                    Console.WriteLine("GEN {0}", i);
                    Console.WriteLine(app.population.ToString());

                    if (bestFitness > app.population.BestFitness)
                        bestFitness = app.population.BestFitness;

                    //Generate the new one; Journey : Montpellier, Grenoble, Mulhouse, Valenciennes, Paris, Nantes,  Fitness : 1470216.5560350444
                    app.NextGen();
                }

                //Print the last generation
                Console.WriteLine("GEN 100");
                Console.WriteLine(app.population.ToString());

                Console.WriteLine("BestFitness Found = {0}", bestFitness);

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

                DataBaseManager.CloseConnection();
            }
        }

        private void NextGen()
        {
            //Generate the next generation
            Population nextPopulation = populationManager.NextGen(population, Utils.SelectionMethodes.Tournament);

            //store it in the population variable
            population = nextPopulation;
        }
    }
}
