using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace PTS.API.Result.Result
{
    [DataContract]
    public class RGetResults
    {
        [DataMember]
        public readonly Dictionary<string, string> cities;
        [DataMember]
        public readonly List<Tuple<string, double>> selectionMethods;
        public RGetResults(List<Tuple<string, double>> selectionMethodes, Dictionary<string, string> cities)
        {
            this.cities = new Dictionary<string, string>(cities);
            this.selectionMethods = new List<Tuple<string, double>>(selectionMethodes);
        }
    }
}
