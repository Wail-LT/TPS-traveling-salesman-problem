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

        public Journey NextJourney(List<string> cities)
        {

            SelectionMethodes.Random.Next();
            throw new NotImplementedException();
        }

    }
}
