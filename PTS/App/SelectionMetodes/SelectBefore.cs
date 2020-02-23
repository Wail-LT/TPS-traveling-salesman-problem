using PTS.App.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTS.App.SelectionMetodes
{
    public class SelectBefore : SelectionMethode
    {
        public new const double D_MUTATE_FACTOR = 0.3;

        private static List<int> indexMates = new List<int>();
        private static bool goodParent = true;

        public SelectBefore(double mFactor = D_MUTATE_FACTOR) : base(mFactor) { }

        //Needed for reflexion
        public SelectBefore() : base(D_MUTATE_FACTOR) { }

        public override Route Selection(List<Route> Routes)
        {
            Random random = Utils.Utils.Random;

            //The 15% Routes
            int pivot = (int)(0.15 * Routes.Count);
            List<Route> bestRoutes = Routes.Take(pivot).ToList();
            List<Route> badRoutes = Routes.Skip(pivot).ToList();

            // List<int> indexMates = Stochastique(badRoutes, pivot);
            // List<Route> mates =
            if (indexMates == null || indexMates.Count == 0)
                indexMates = Stochastique(badRoutes, pivot);
            Random randomIndex = Utils.Utils.Random;
            Route result;
            if (goodParent)
            {
                int indexSorti = randomIndex.Next(0, bestRoutes.Count);
                result = bestRoutes[indexSorti];
            }
            else
            {
                int indexSorti = randomIndex.Next(0, indexMates.Count);
                result = badRoutes[indexMates[indexSorti]];
            }

            goodParent = !goodParent;

            return result;
        }

        private static List<int> Stochastique(List<Route> routes, int nbReproduction)
        {
            double fitnessTotal = 0;
            List<double> pourcentageCumuler = new List<double>();
            List<int> indexReproduction = new List<int>();
            Random random = Utils.Utils.Random;

            //allows to have the total of the fitness
            for (int i = 0; i < routes.Count; i++)
            {
                fitnessTotal = fitnessTotal + routes[i].Fitness;
            }
            //on calcule le pourcentage cumulé de chaque fitness pr avoir la répartition de chaque fitness
            pourcentageCumuler.Add(routes[0].Fitness / fitnessTotal);
            for (int i = 1; i < routes.Count; i++)
            {
                pourcentageCumuler.Add((routes[0].Fitness / fitnessTotal) + pourcentageCumuler[i - 1]);
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

                    if (currentIndex == pourcentageCumuler.Count - 1)
                        return randomPourcentage >= pourcentage;
                    else if (currentIndex > 0)
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
