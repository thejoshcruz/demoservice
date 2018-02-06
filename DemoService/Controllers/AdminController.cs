using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Couchbase;
using Couchbase.Configuration.Client;
using Couchbase.Authentication;

using DemoService.Data;
using DemoService.Exceptions;
using DemoService.Models;

namespace DemoService.Controllers
{
    /// <summary>
    /// the admin controller where all admin stuff lives
    /// </summary>
    [Route("api/[controller]")]
    public class AdminController : BaseController
    {
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="dataProcessor">the processors to use when performing actions with data</param>
        public AdminController(IDataProcessor dataProcessor)
            :base(dataProcessor)
        { }

        /// <summary>
        /// provides the ability to check if the service is up and accepting requests
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /Ping
        ///     {
        ///       "echo": woohoo!
        ///     }
        ///     
        /// </remarks>
        /// <param name="echo">input string to return in the response</param>
        /// <returns>Returns a response string that includes the input string</returns>
        [HttpGet("Ping")]
        public string Ping([FromQuery]string echo)
        {
            return $"Received {echo} at {DateTime.Now.ToString("HH:mm:ss")}";
        }

      
    }
}
