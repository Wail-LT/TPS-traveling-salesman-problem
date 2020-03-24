using PTS.App.Objects;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PTS.API.Result.Result
{
    [DataContract()]
    public class PGetResults
    {
        [DataMember()]
        public int sessionToken;
        [DataMember()]
        public List<City> cities;
        [DataMember()]
        public List<string> selectionMethods;
        [DataMember()]
        public List<float> mutationFactors;
        [DataMember()]
        public int nbGenerations;

        [DataMember()]
        public int amountOfCities;


        public PGetResults(int sessionToken, List<string> selectionMethods, List<City> cities, List<float> mutationFactors, int nbGenerations, int amountOfCities)
        {
            this.sessionToken = sessionToken;
            this.cities = new List<City>(cities);
            this.selectionMethods = new List<string>(selectionMethods);
            this.mutationFactors = mutationFactors;
            this.nbGenerations = nbGenerations;
            this.amountOfCities = amountOfCities;
        }

        public PGetResults() { }
    }
}
