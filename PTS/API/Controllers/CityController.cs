using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PTS.App.Managers;
using PTS.App.Objects;

namespace PTS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CityController : ControllerBase
    {
        private CityManager cityManager = new CityManager();

        [HttpGet]
        public IEnumerable<Tuple<string, string>> GetAll()
        {
            List<Tuple<string,string>> a = cityManager.GetAllCitiesName();

            return a;
        }
    }
}
