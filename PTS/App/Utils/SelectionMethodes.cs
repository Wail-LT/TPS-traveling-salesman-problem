using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using PTS.App.Objects;
using System.Linq;

namespace PTS.App.Utils
{
    public static class SelectionMethodes
    {
        public static Journey Tournament(List<Journey> journeys)
        {
            List<int> Participant = new List<int>();

            //Create Tournament between 15% of the journeys
            int size = (int)(journeys.Count * 0.15);

            Random random = Utils.Random;

            //First participant
            int index = random.Next(0, journeys.Count);
            //Add it to the list of participant
            Participant.Add(index);

            //Index of new participants 
            int rIndex;

            for (int i = 0; i < size - 1; i++)
            {
                do
                {
                    //New random participant
                    rIndex = random.Next(0, journeys.Count);
                } while (Participant.Contains(rIndex));

                //Add it to the list of participant
                Participant.Add(rIndex);

                //If fitess of the new participant is better than the current best one
                //then save it as better participant
                if (journeys[index].Fitness > journeys[rIndex].Fitness)
                {
                    index = rIndex;
                }
            }

            return journeys[index];
        }

        public static List<Journey> Elitist(List<Journey> journeys, int nb)
        {
            List<Journey> bestJourneys = journeys.OrderBy(j => j.Fitness).ToList();
           
            return bestJourneys.Take(nb).ToList();
        }
    }
}
