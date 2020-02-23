using PTS.App.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTS.App.SelectionMetodes
{
    public abstract class SelectionMethode
    {
        public const double D_MUTATE_FACTOR = 0;
        public readonly double mutateFactor;
        public SelectionMethode(double mFactor) { mutateFactor = mFactor; }

        public abstract Route Selection(List<Route> routes);


    }
}
