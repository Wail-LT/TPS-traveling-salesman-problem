using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PTS.App.Objects;
using PTS.API;
using PTS.App.SelectionMetodes;
using PTS.App.Managers;
using System.Runtime.Serialization.Json;
using System.IO;
using PTS.API.Result.Result;
using PTS.API.Controllers.Result.Result;
using PTS.App.Utils;
using Newtonsoft.Json.Linq;

namespace PTS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class RandomResultController : ControllerBase
    {

        private Population population;

        [HttpPost]
        public string GetResults([FromBody] Object jsonParams)
        {
            string json = "{}";
            //try
           // {
                JObject jsonObj = JObject.Parse(jsonParams.ToString());
                PGetResults parameters = jsonObj.ToObject<PGetResults>();

                //Get session from token
                Session session = SessionManager.GetSession(parameters.sessionToken);

                //Getting random cities
                Dictionary<string, string> cities = CityManager.GetCitiesNumber(parameters.amountOfCities);

                //Set population from cities
                session.SetPopulationManager(cities);

                Type methodType;
                SelectionMethode selectionMethod;
                List<List<double>> bestFitnessPerGen = new List<List<double>>();
                List<List<City>> bestRoutePerMethod = new List<List<City>>();

                //Run algorithm for each selected method
                int methodeIndex = 0;
                foreach (string method in parameters.selectionMethods)
                {
                    //Get the type of the selection methodes
                    methodType = Type.GetType("PTS.App.SelectionMetodes." + method);
                    //Instanciate the selectionMethode
                    selectionMethod = (SelectionMethode)Activator.CreateInstance(methodType);
                    //Get mutation factor
                    double mutationFactor = parameters.mutationFactors[methodeIndex];

                    Func<List<City>, int, Population> iniFunc = null;
                    //Generate first population
                    if (selectionMethod is SelectBefore)
                        iniFunc = IniFunctions.AdamAndEve;
                    //Generate first population
                    population = session.GetPopulationManager().GeneratePopulation(iniFunc);
                    int nbGenerations = parameters.nbGenerations;

                    List<double> bestFitnesses = new List<double>();
                    Route BestRoute = null;
                    for (int i = 0; i < nbGenerations; i++)
                    {
                        //Generate the new one; 
                        //Generate the next generation
                        Population nextPopulation = session.GetPopulationManager().NextGen(population, selectionMethod.Selection, mutationFactor);
                        //store it in the population variable
                        population = nextPopulation;

                        bestFitnesses.Add(population.BestFitness);
                        if (BestRoute == null || BestRoute.Fitness > population.BestFitness)
                            BestRoute = population.BestRoute;
                    }

                    bestFitnessPerGen.Add(bestFitnesses);
                    bestRoutePerMethod.Add(BestRoute.Cities);
                    methodeIndex++;
                }


                json = Utils.SerializeObj<RGetResults>(new RGetResults(bestFitnessPerGen, bestRoutePerMethod));
                

            /*}catch(Exception e){
                json = e.Message;
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }*/


            return json;
        }


        private void NextGen(PopulationManager populationManager, Func<List<Route>, Route> selectionMethode, double mutateFactor)
        {
            //Generate the next generation
            Population nextPopulation = populationManager.NextGen(population, selectionMethode, mutateFactor);

            //store it in the population variable
            population = nextPopulation;
        }

    }
}
