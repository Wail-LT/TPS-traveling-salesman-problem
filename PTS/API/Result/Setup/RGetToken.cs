using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace PTS.API.Result.Setup
{
    [DataContract]
    public class RGetToken
    {
        [DataMember]
        public readonly int token;
        [DataMember]
        public readonly List<Tuple<string, double>> selectionMethodes;
        [DataMember]
        public readonly List<Tuple<string, string>> cityList;
        public RGetToken(int token, List<Tuple<string, double>> selectionMethodes, List<Tuple<string, string>> cityList)
        {
            this.token = token;
            this.selectionMethodes = new List<Tuple<string, double>>(selectionMethodes);
            this.cityList = new List<Tuple<string, string>>(cityList);
        }
    }
}
