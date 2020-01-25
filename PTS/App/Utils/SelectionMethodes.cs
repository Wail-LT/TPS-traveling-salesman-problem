using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using PTS.App.Objects;
using System.Linq;

namespace PTS.App.Utils
{
    public static class SelectionMethodes
    {
        private static double ELITIST_OFFSET = 0.15;
        private static List<int> indexMates = new List<int>();

        public static Route Tournament(List<Route> routes)
        {
            List<int> Participant = new List<int>();

            //Create Tournament between 15% of the routes
            int size = (int)(routes.Count * 0.15);

            Random random = Utils.Random;

            //First participant
            int index = random.Next(0, routes.Count);
            //Add it to the list of participant
            Participant.Add(index);

            //Index of new participants 
            int rIndex;

            for (int i = 0; i < size - 1; i++)
            {
                do
                {
                    //New random participant
                    rIndex = random.Next(0, routes.Count);
                } while (Participant.Contains(rIndex));

                //Add it to the list of participant
                Participant.Add(rIndex);

                //If fitess of the new participant is better than the current best one
                //then save it as better participant
                if (routes[index].Fitness > routes[rIndex].Fitness)
                {
                    index = rIndex;
                }
            }

            return routes[index];
        }

        public static Route Elitist(List<Route> routes)
        {
            Random random = Utils.Random;

            List<Route> bestRoutes = routes.OrderBy(j => j.Fitness)
                                                 .Take((int)(routes.Count * ELITIST_OFFSET))
                                                 .ToList();

            return bestRoutes[random.Next(bestRoutes.Count)];
        }

        private static  List<int> Stochastique(List<Journey> journeys,int nbReproduction)
        {
            double fitnessTotal = 0;
            List<double> pourcentageCumuler = new List<double>();
            List<int> indexReproduction = new List<int>();
            Random random = Utils.Random;
           
            //allows to have the total of the fitness
            for (int i=0; i<journeys.Count; i++)
            {
                fitnessTotal = fitnessTotal + journeys[i].Fitness;
            }
            //on calcule le pourcentage cumulé de chaque fitness pr avoir la répartition de chaque fitness
            pourcentageCumuler.Add(journeys[0].Fitness / fitnessTotal);
            for (int i = 1; i < journeys.Count; i++)
            {
                pourcentageCumuler.Add(( journeys[0].Fitness / fitnessTotal) + pourcentageCumuler[i - 1]);              
            }

            int parentIndex;
            double randomPourcentage;

            //nous permet de trouver l'index du nbre correspodant au aléatoire qu'on tire, si on tire 5,2 ca nous retourne lindex du nombre qui correspond à la ou il y a 52%
            for (int i = 0; i < nbReproduction; i++)
            {
                randomPourcentage = random.NextDouble();
                parentIndex = pourcentageCumuler.IndexOf(pourcentageCumuler.Find(pourcentage =>
                {
                    int currentIndex = pourcentageCumuler.IndexOf(pourcentage);

                    if (currentIndex > 0)
                        return pourcentageCumuler[currentIndex - 1] > randomPourcentage && randomPourcentage <= pourcentage;
                    else
                        return randomPourcentage <= pourcentage;

                }));

                indexReproduction.Add(parentIndex);
            }
            return indexReproduction;

        }

        //Method "BEFORE"
        public static Journey PreSelect(List<City> cities)
        {
            List<double> totalDistances = new List<double>(cities.Count);
            List<City> parentJourney = new List<City>(cities.Count);
            List<City> adjacentCities = new List<City>(cities);

            //Calculate total sum of distances from each city to the others
            for (int i = 0; i < adjacentCities.Count; i++)
            {
                double dist = 0;
                for (int j = 0; j < adjacentCities.Count; j++)
                {
                    if (i != j)
                        dist += adjacentCities[i].GetDistanceTo(adjacentCities[j]);
                }
                totalDistances.Add(dist);
            }

            double minDist = totalDistances.Min();
            City startCity = adjacentCities[totalDistances.IndexOf(minDist)];
            City lastCity = startCity;
            parentJourney.Add(startCity);
            adjacentCities.Remove(startCity);

            List<double> delta = new List<double>(adjacentCities.Count);
            for (int i = 0; i < adjacentCities.Count; i++)
            {
                City nextCity = Journey.CompareCities(startCity, lastCity, adjacentCities);
                parentJourney.Add(nextCity);
                adjacentCities.Remove(nextCity);
                lastCity = nextCity;
            }

            return new Journey(parentJourney);

        }
        public static int SelectBefore(List<Journey> journeys, )
        {
            Random random = Utils.Random;

            //The 15% journeys 
            int pivot = (int)(0.15 * journeys.Count);
            List<Journey> bestJourneys = journeys.Take(pivot).ToList();
            List<Journey> badJourneys = journeys.Skip(pivot).ToList();

           // List<int> indexMates = Stochastique(badJourneys, pivot);
           // List<Journey> mates =
           if (SelectionMethodes.indexMates==null || SelectionMethodes.indexMates.Count==0)
                indexMates = Stochastique(badJourneys, pivot);
                Random randomIndex = Utils.Random;
                int indexSorti = randomIndex.Next(0, indexMates.Count);
            return indexMates[indexSorti]; //A REVOIR car rertourne index faurdrai retourner un trajet merci a bientot 
        }
    }
}
