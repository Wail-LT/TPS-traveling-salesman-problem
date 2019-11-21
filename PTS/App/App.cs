using System;
using PTS.App.Managers;
using PTS.App.Objects;
using PTS.App.DataBase;

namespace PTS.App
{
    public class App
    {
        //Managers
        private PopulationManager populationManager;

        //App components
        private Population population;
        
        private App(int dbConn)
        {
            //1. init the populationManager with connection to the database
            populationManager = new PopulationManager(dbConn);

            //2. Generate the first population
            population = populationManager.GeneratePopulation();
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
            DataBaseManager.SetupConnection();

            //init the app with database connection
            App app = new App(DataBaseManager.connection);

            double bestFitness = app.GetPopulation().GetBestFitness();

            for (int i = 0; i < nbGeneration; i++)
            {
                //Get and print the best fitness of the generation and the generation number
                Console.WriteLine("Generation {0}: {1};", i, app.GetPopulation().GetBestFitness());

                //Store the fitness if it's better than the previous one
                if (bestFitness < app.GetPopulation().GetBestFitness())
                    bestFitness = app.GetPopulation().GetBestFitness();

                //Generate the next generation
                app.NextGen();
            }
                
        }
    }
}
