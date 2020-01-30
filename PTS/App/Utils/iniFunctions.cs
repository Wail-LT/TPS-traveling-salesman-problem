using System;
using System.Collections.Generic;
using System.Linq;
using PTS.App.Objects;

namespace PTS.App.Utils
{
    public static class IniFunctions
    {
        /*Public Static Functions*/

        //Method "BEFORE"
        public static Route PreSelect(List<City> cities)
        {
            List<double> totalDistances = new List<double>(cities.Count);
            List<City> parentRoute = new List<City>(cities.Count);
            List<City> adjacentCities = new List<City>(cities);

            //Calculate total sum of distances from each city to the others
            for (int i = 0; i < adjacentCities.Count; i++)
            {
                double dist = 0;
                for (int j = 0; j < adjacentCities.Count; j++)
                {
                    if (i != j)
                        dist += adjacentCities[i].GetDistanceTo(adjacentCities[j]);
                }
                totalDistances.Add(dist);
            }

            double minDist = totalDistances.Min();
            City currentCity = adjacentCities[totalDistances.IndexOf(minDist)];
            City lastCity = currentCity;
            parentRoute.Add(currentCity);
            adjacentCities.Remove(currentCity);

            List<double> delta = new List<double>(adjacentCities.Count);
            for (int i = 0; i < adjacentCities.Count; i++)
            {
                City nextCity = CompareCities(currentCity, lastCity, adjacentCities);
                parentRoute.Add(nextCity);
                adjacentCities.Remove(nextCity);
                currentCity = nextCity;
            }
            parentRoute.Add(adjacentCities[0]);

            //Console.WriteLine(string.Join(",", adjacentCities));

            return new Route(parentRoute);
        }


        /*Private Static Functions*/
        private static double Delta(List<double> val1, List<double> val2)
        {
            return val1[0] - val2[0] + val1[1] - val2[1] + val1[2] - val2[2];
        }

        private static City CompareCities(City refCity, City lastCity, List<City> others)
        {
            Dictionary<City, List<double>> infos = new Dictionary<City, List<double>>();

            City bestCity;
            if (others.Count > 2)
            {
                //Gathering data
                for (int i = 0; i < others.Count; i++)
                {
                    double distToLast = others[i].GetDistanceTo(lastCity);
                    double nearest = -1;
                    double furthest = 0;

                    for (int j = 0; j < others.Count; j++)
                    {
                        if (i != j)
                        {
                            double distToNeighbor = others[i].GetDistanceTo(others[j]);
                            if (distToNeighbor < nearest || nearest == -1)
                                nearest = distToNeighbor;
                            else if (distToNeighbor > furthest)
                                furthest = distToNeighbor;
                        }
                    }
                    infos.Add(others[i], new List<double>() { distToLast, nearest, furthest });
                }

                //Comparing cities
                double delta = 0;
                List<City> citiesToCompare = new List<City>(others);
                bestCity = citiesToCompare[0];

                for (int i = 1; i < citiesToCompare.Count; i++)
                {
                    delta = Delta(infos[bestCity], infos[citiesToCompare[i]]);
                    if (delta > 0)
                        bestCity = citiesToCompare[i];
                    else if (delta == 0)
                    {
                        //should use infos tab instead recalculating dist
                        double dist1 = bestCity.GetDistanceTo(lastCity);
                        double dist2 = citiesToCompare[i].GetDistanceTo(lastCity);
                        if (dist1 > dist2)
                            bestCity = citiesToCompare[i];
                    }
                }
            }
            else if (others.Count == 2)
            {
                //problem here
                //For the last 2 cities
                double deltaToRef = others[0].GetDistanceTo(refCity) - others[1].GetDistanceTo(refCity);
                double deltaToLast = others[0].GetDistanceTo(lastCity) - others[1].GetDistanceTo(lastCity);

                double delta = Math.Abs(deltaToRef) - Math.Abs(deltaToLast);

                if (delta < 0)
                {
                    if (deltaToLast < 0)
                        bestCity = others[1];
                    else
                        bestCity = others[0];
                }
                else
                {
                    if (deltaToRef < 0)
                        bestCity = others[0];
                    else
                        bestCity = others[1];
                }
                /*
                double delta = Math.Abs(deltaToRef)-

                double delta = others[0].GetDistanceTo(lastCity) - others[1].GetDistanceTo(lastCity) +
                    others[0].GetDistanceTo(refCity) - others[1].GetDistanceTo(refCity);
                if (delta > 0)  //this time we take the biggest value
                    bestCity = others[0];
                else bestCity = others[1];*/
            }
            else bestCity = others[0];

            return bestCity;

        }
    }
}
