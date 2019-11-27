using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using PTS.App.Objects;
using System.Linq;

namespace PTS.App.Utils
{
    public static class SelectionMethodes
    {
        private static Random random = new Random();

        public static Random Random => random;

        public static Journey Tournament(List<Journey> journeys, int size)
        {
            List<int> Participant = new List<int>();

            int index = -1;
            int rIndex;

            for ( int i = 0; i < size; i++)
            {
                do
                {
                    rIndex = random.Next(0, journeys.Count);
                } while (Participant.Contains(rIndex));

                Participant.Add(rIndex);

                if ( index != -1 && journeys[index].GetFitness() > journeys[rIndex].GetFitness())
                {
                    index = rIndex;
                }
            }

            return journeys[index];
        }

        public static List<Journey> Elitist(List<Journey> journeys, int nb)
        {
            List<Journey> bestJourneys = journeys.OrderBy(j => j.GetFitness()).ToList();
           
            return bestJourneys.Take(nb).ToList();
        }
    }
}
