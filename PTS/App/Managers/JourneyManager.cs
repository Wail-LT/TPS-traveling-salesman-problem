using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using PTS.App.Objects;
using PTS.App.Utils;

namespace PTS.App.Managers
{
    public class JourneyManager
    {
        public JourneyManager(MySql.Data.MySqlClient.MySqlConnection dbConn)
        {
        }

        public Journey NextJourney(Dictionary<string, string> cities)
        {
            //recuperer les key du dicctionaire pour les melanger et récuppereler un trajet au hazard
            //Randomize the cities
            cities.Shuffle();
            
            return null;
        }

    }
}
