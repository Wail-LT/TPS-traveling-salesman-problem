using PTS.App.Objects;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PTS.API.Controllers.Result.Result
{
    [DataContract()]
    public class RGetResults
    {
        [DataMember()]
        public readonly IEnumerable<IEnumerable<double>> bestFitnessPerGen;

        [DataMember()]
        public readonly IEnumerable<IEnumerable<City>> bestRoutePerMethod;

        public RGetResults(IEnumerable<IEnumerable<double>> bestFitnessPerGen, IEnumerable<IEnumerable<City>> bestRoutePerMethod)
        {
            this.bestFitnessPerGen = bestFitnessPerGen;
            this.bestRoutePerMethod = bestRoutePerMethod;
        }
    }
}
