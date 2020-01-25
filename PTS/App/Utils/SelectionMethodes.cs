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
    }
}
