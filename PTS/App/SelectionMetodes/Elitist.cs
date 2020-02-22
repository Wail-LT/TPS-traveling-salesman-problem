using PTS.App.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTS.App.SelectionMetodes
{
    public class Elitist : SelectionMethode
    {
        private const double D_MUTATE_FACTOR = 0.3;
        private static double ELITIST_OFFSET = 0.15;
        public Elitist(double mFactor = D_MUTATE_FACTOR) : base(mFactor) { }
        public override Route Selection(List<Route> routes)
        {
            Random random = Utils.Utils.Random;

            List<Route> bestRoutes = routes.OrderBy(j => j.Fitness)
                                                 .Take((int)(routes.Count * ELITIST_OFFSET))
                                                 .ToList();

            return bestRoutes[random.Next(bestRoutes.Count)];
        }
    }
}
