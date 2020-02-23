using System;
using PTS.App.Managers;
using PTS.App.Objects;
using PTS.App.DataBase;
using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Collections.Generic;
using PTS.App.Utils;
using PTS.App.SelectionMetodes;

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
            populationManager = new PopulationManager(cities);

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
                //Set the list of city
                Dictionary<string, string> cities = new Dictionary<string, string>();

                cities.Add("69001", "Lyon");
                cities.Add("59000", "Lille");
                cities.Add("75001", "Paris");
                cities.Add("44100", "Nantes");
                cities.Add("13001", "Marseille");
                cities.Add("68100", "Mulhouse");
                cities.Add("59300", "Valenciennes");
                cities.Add("06100", "Nice");
                cities.Add("14100", "Lisieux");
                cities.Add("57000", "Metz");
                cities.Add("33310", "Lormont");
                cities.Add("31000", "Toulouse");

                //init the app with database connection
                App app = new App(DataBaseManager.Connection, cities);

                Type methodeType;
                SelectionMethode selectionMethode;
                Route bestRoute;
                int bestGen = 0;

                foreach (ESelectionMethodes eMethode in Enum.GetValues(typeof(ESelectionMethodes)))
                {
                    Console.WriteLine(eMethode.ToString());

                    //Get the type of the selection methodes
                    methodeType = Type.GetType("PTS.App.SelectionMetodes."+eMethode.ToString());

                    //Instanciate the selectionMethode
                    selectionMethode = (SelectionMethode)Activator.CreateInstance(methodeType);

                    //Generate first population
                    app.population = app.populationManager.GeneratePopulation();

                    bestRoute = app.population.BestRoute;
                    bestGen = 0;


                    for (int i = 0; i < 99; i++)
                    {
                        if (bestRoute.Fitness > app.population.BestFitness)
                        {
                            bestRoute = app.population.BestRoute;
                            bestGen = i;
                        }

                        //Generate the new one; 
                        app.NextGen(selectionMethode.Selection);
                    }

                    if (bestRoute.Fitness > app.population.BestFitness)
                    {
                        bestRoute = app.population.BestRoute;
                        bestGen = 100;
                    }

                    Console.WriteLine("Best Route Found : \n " +
                        "{0}\n" +
                        "Generation : {1}",
                        bestRoute,
                        bestGen);
                }

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
