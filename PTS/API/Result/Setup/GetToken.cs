using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTS.API.Result.Setup
{
    public class GetToken
    {
        public readonly int token;
        public readonly List<Tuple<string, double>> selectionMethodes;
        public readonly List<Tuple<string, string>> cityList;
        public GetToken(int token, List<Tuple<string, double>> selectionMethodes, List<Tuple<string, string>> cityList)
        {
            this.token = token;
            this.selectionMethodes = new List<Tuple<string, double>>(selectionMethodes);
            this.cityList = new List<Tuple<string, string>>(cityList);
        }
    }
}
