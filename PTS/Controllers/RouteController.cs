using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PTS.App.Managers;
using PTS.App.Objects;

namespace PTS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RouteController : ControllerBase
    {
        private CityManager cityManager = new CityManager(App.DataBase.DataBaseManager.Connection);

        [HttpPost]
        public string GetBestRoute(string str)
        {
            return str;
        }
    }
}
