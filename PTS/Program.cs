using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PTS.App;
using PTS.App.DataBase;

namespace PTS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Setup the database connection
            try
            {
                DataBaseManager.SetupConnection();
                CreateWebHostBuilder(args).Build().Run();
                App.App.Start(20);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
