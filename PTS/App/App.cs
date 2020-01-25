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

            if (DataBaseManager.Connection != null)
            {

                Dictionary<string, string> cities = new Dictionary<string, string>();

                cities.Add("34000", "Montpellier");
                cities.Add("38100", "Grenoble");
                cities.Add("75001", "Paris");
                cities.Add("44100", "Nantes");
                cities.Add("68100", "Mulhouse");
                cities.Add("59300", "Valenciennes");
                cities.Add("06100", "Nice");
                cities.Add("14100", "Lisieux");
                cities.Add("57000", "Metz");
                cities.Add("33310", "Lormont");
                cities.Add("31000", "Toulouse");

                Console.WriteLine("TOURNOI");
                //init the app with database connection
                App app = new App(DataBaseManager.Connection, cities);

                Route bestCity = app.population.BestRoute;
                int bestGen = 0;

                //Print the first population
                Console.WriteLine("GEN {0}", 1);
                Console.WriteLine(app.population.ToString());

                for (int i = 0; i < 99; i++)
                {
                    if (bestCity.Fitness > app.population.BestFitness)
                    {
                        //Print the current population
                        Console.WriteLine("GEN {0}", i);
                        Console.WriteLine(app.population.ToString());

                        bestCity = app.population.BestRoute;
                        bestGen = i;
                    }
                    
                    //Generate the new one; Journey : Montpellier, Grenoble, Mulhouse, Valenciennes, Paris, Nantes,  Fitness : 1470216.5560350444
                    app.NextGen(Utils.SelectionMethodes.Tournament);
                }

                if (bestCity.Fitness > app.population.BestFitness)
                {
                    //Print the last generation
                    Console.WriteLine("GEN 100");
                    Console.WriteLine(app.population.ToString());

                    bestCity = app.population.BestRoute;
                    bestGen = 100;
                }

                Console.WriteLine("Best Route Found : \n " +
                    "{0}\n" +
                    "Generation : {1}",
                    bestCity,
                    bestGen);


                Console.WriteLine("ELITISTE");
                //init the app with database connection
                app = new App(DataBaseManager.Connection, cities);

                bestCity = app.population.BestRoute;
                bestGen = 0;


                //Print the first population
                Console.WriteLine("GEN {0}", 1);
                Console.WriteLine(app.population.ToString());

                for (int i = 0; i < 99; i++)
                {
                    if (bestCity.Fitness > app.population.BestFitness)
                    {
                        //Print the current population
                        Console.WriteLine("GEN {0}", i);
                        Console.WriteLine(app.population.ToString());

                        bestCity = app.population.BestRoute;
                        bestGen = i;
                    }


                    //Generate the new one; Route : Montpellier, Grenoble, Mulhouse, Valenciennes, Paris, Nantes,  Fitness : 1470216.5560350444
                    app.NextGen(Utils.SelectionMethodes.Elitist);
                }

                if (bestCity.Fitness > app.population.BestFitness)
                {
                    //Print the last generation
                    Console.WriteLine("GEN 100");
                    Console.WriteLine(app.population.ToString());

                    bestCity = app.population.BestRoute;
                    bestGen = 100;
                }

                Console.WriteLine("Best Route Found : \n " +
                    "{0}\n" +
                    "Generation : {1}",
                    bestCity,
                    bestGen);
                DataBaseManager.CloseConnection();
            }
        }

        private void NextGen(Func<List<Route>, Route> selectionMethode)
        {
            //Generate the next generation
            Population nextPopulation = populationManager.NextGen(population, selectionMethode, 0.1);

            //store it in the population variable
            population = nextPopulation;
        }
    }
}
