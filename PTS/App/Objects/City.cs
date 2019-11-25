using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoCoordinatePortable;

namespace PTS.App.Objects
{

    public class City
    {
        private GeoCoordinate coordinates;  //latitude and longitude

        public GeoCoordinate Coordinates => coordinates;

        public double GetDistance(City c)
        {
            return coordinates.GetDistanceTo(c.Coordinates);
        }
    }

    
}
