using System;
using PTS.App.Managers;
using PTS.App.Objects;
using PTS.App.DataBase;
using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Collections.Generic;
using PTS.App.Utils;
using PTS.App.SelectionMetodes;
using System.Diagnostics;

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
                //Dictionary<string, string> cities = new Dictionary<string, string>();
                Dictionary<string, string> cities = CityManager.GetCitiesNumber(300);
                
                //init the app with database connection
                App app = new App(DataBaseManager.Connection, cities);

                Type methodeType;
                SelectionMethode selectionMethode;
                Route bestRoute;
                int bestGen = 0;

                foreach (ESelectionMethodes eMethode in Enum.GetValues(typeof(ESelectionMethodes)))
                {
                    double execTime = 0;
                    double fitness = 0;
                    int bestGenMean = 0;

                    for (int j = 0; j < 4; j++) 
                    {
                        Stopwatch stopwatch = new Stopwatch();
                        stopwatch.Start();
                        Console.WriteLine(eMethode.ToString());

                        //Get the type of the selection methodes
                        methodeType = Type.GetType("PTS.App.SelectionMetodes." + eMethode.ToString());

                        //Instanciate the selectionMethode
                        selectionMethode = (SelectionMethode)Activator.CreateInstance(methodeType);

                        Func<List<City>, int, Population> iniFunc = null;
                        //Generate first population
                        if (selectionMethode is SelectBefore)
                            iniFunc = Utils.IniFunctions.AdamAndEve;
                        app.population = app.populationManager.GeneratePopulation(iniFunc);

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
                            app.NextGen(selectionMethode.Selection, selectionMethode.mutateFactor);
                        }

                        if (bestRoute.Fitness > app.population.BestFitness)
                        {
                            bestRoute = app.population.BestRoute;
                            bestGen = 100;
                        }

                        stopwatch.Stop();
                        execTime += stopwatch.Elapsed.TotalSeconds;
                        fitness += bestRoute.Fitness;
                        bestGenMean += bestGen;
                    }
                    

                    Console.WriteLine("Best Fitness Found : \n " +
                        "{0}\n" +
                        "Generation : {1}",
                        Math.Round(fitness / 4000, 2),
                        bestGenMean/ 4);
            
                    Console.WriteLine(fitness / 4);
                    Console.WriteLine("Durée d'exécution: {0}",execTime/4.0);
                }

                DataBaseManager.CloseConnection();
            }
        }

        private void NextGen(Func<List<Route>, Route> selectionMethode, double mutateFactor)
        {
            //Generate the next generation
            Population nextPopulation = populationManager.NextGen(population, selectionMethode, mutateFactor);

            //store it in the population variable
            population = nextPopulation;
        }
    }
}
