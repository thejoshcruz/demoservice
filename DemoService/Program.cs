using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using DemoService.Data;

namespace DemoService
{
    /// <summary>
    /// the main program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// main method
        /// </summary>
        /// <param name="args">input args</param>
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        /// <summary>
        /// builds the web host
        /// </summary>
        /// <param name="args">input args</param>
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
