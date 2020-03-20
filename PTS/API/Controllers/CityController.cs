using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PTS.App.Managers;
using PTS.App.Objects;
using PTS.App.Utils;

namespace PTS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CityController : ControllerBase
    {
        private CityManager cityManager = new CityManager();

        [HttpGet, Route("")]
        public String GetAll()
        {
            return Utils.SerializeObj<List<City>>(cityManager.GetAllCitiesName());
        }

        [HttpGet, Route("location")]
        public String GetCities(String text)
        {
            return Utils.SerializeObj<List<City>>(cityManager.GetCitiesName(text));
        }
    }
}
