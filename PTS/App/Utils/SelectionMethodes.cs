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

        public static Journey Elitist(List<Journey> journeys)
        {
            Random random = Utils.Random;

            List<Journey> bestJourneys = journeys.OrderBy(j => j.Fitness)
                                                 .Take((int)(journeys.Count * ELITIST_OFFSET))
                                                 .ToList();

            return bestJourneys[random.Next(bestJourneys.Count)];
        }

        private static List<int> Stochastique(List<Journey> journeys,int nbReproduction)
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
    }
}
