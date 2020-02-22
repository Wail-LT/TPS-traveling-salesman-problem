using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PTS.App.Managers;
using PTS.App.Objects;

namespace PTS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SetupController : ControllerBase
    {
        private CityManager cityManager = new CityManager();

        [HttpGet]
        public IEnumerable<Dictionary<string, string>> GetToken()
        {
            

            return a;
        }
    }
}
