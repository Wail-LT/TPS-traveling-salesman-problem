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

namespace PTS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResultController : ControllerBase
    {

        private Population population;

        [HttpGet]
        public string GetResults(int sessionToken, RGetResults resultData, int nbGenerations)
        {
            //Get session from token
            Session session = SessionManager.GetSession(sessionToken);

            //Set population from cities
            session.SetPopulationManager(resultData.cities);

            Type methodType;
            SelectionMethode selectionMethod;
            List<List<Route>> allRoutes = new List<List<Route>>();   //the routes for every generation of each method
            //int bestGen = 0;

            //Run algorithm for each selected method
            foreach (Tuple<string, double> method in resultData.selectionMethods)
            {
                //Get the type of the selection methodes
                methodType = Type.GetType("PTS.App.SelectionMetodes." + method.Item1);

                //Instanciate the selectionMethode
                selectionMethod = (SelectionMethode)Activator.CreateInstance(methodType);

                //Generate first population
                population = session.GetPopulationManager().GeneratePopulation();

                List<Route> routes = new List<Route>();
                for (int i = 0; i < nbGenerations; i++)
                {
                    //Generate the new one; 
                    NextGen(session.GetPopulationManager(), selectionMethod.Selection, method.Item2);
                    routes.Add(population.BestRoute);
                }
                allRoutes.Add(routes);

            }

            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(List<List<Route>>));
            MemoryStream msObj = new MemoryStream();
            js.WriteObject(msObj, allRoutes);
            msObj.Position = 0;
            StreamReader sr = new StreamReader(msObj);

            string json = sr.ReadToEnd();

            sr.Close();
            msObj.Close();

            return json;
            // return allRoutes;
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
