using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoCoordinatePortable;

namespace PTS.App.Objects
{

    public class City
    {
        public string name;
        private GeoCoordinate coordinates;  //latitude and longitude

        /*Properties*/
        public GeoCoordinate Coordinates => coordinates;

        public City(string name, double longitude, double latitude)
        {
            this.name = name;
            this.coordinates = new GeoCoordinate(latitude, longitude);
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
