using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using GeoCoordinatePortable;

namespace PTS.App.Objects
{
    [DataContract()]
    public class City
    {
        [DataMember()]
        public readonly string name;
        [DataMember()]
        public readonly string zip;

        private GeoCoordinate coordinates;  //latitude and longitude

        /*Properties*/
        public GeoCoordinate Coordinates => coordinates;

        public City(string name, string zip, double longitude = 0, double latitude = 0)
        {
            this.name = name;
            this.coordinates = new GeoCoordinate(latitude, longitude);
            this.zip = zip;
        }

        public double GetDistanceTo(City c)
        {
            return coordinates.GetDistanceTo(c.Coordinates);
        }

        public override string ToString()
        {
            return name ;
        }
    }

    
}
