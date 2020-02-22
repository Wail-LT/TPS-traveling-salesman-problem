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
        public IEnumerable<Dictionary<string, string>> GetAll()
        {
            List<Dictionary<string,string>> a = cityManager.GetAllCitiesName();

            return a;
        }
    }
}
