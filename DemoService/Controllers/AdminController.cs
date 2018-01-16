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

        /// <summary>
        /// adds users, accounts, and portfolios to the proper buckets so we can demo this thing
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /Populate
        ///     {
        ///       "portfolioCount": 20,
        ///       "accountCount": 500,
        ///       "usersCount": 10
        ///     }
        ///     
        /// </remarks>
        /// <param name="accountCount">the number of accounts to add to the AccountState bucket (must be > 0)</param>
        /// <param name="portfolioCount">the number of portfolios to add to the PortfolioState bucket (must be > 0)</param>
        /// <param name="usersCount">the number of users to add to the Users bucket (must be > 0)</param>
        /// <returns>Returns "done" when the records are entered</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Something failed</response>
        [Route("Populate")]
        [HttpPost]
        [ProducesResponseType(typeof(ErrorDetails), 400)]
        public object Populate([FromQuery] int portfolioCount, [FromQuery] int accountCount, [FromQuery] int usersCount)
        {
            if (portfolioCount <= 0 
                || accountCount <= 0
                || usersCount <= 0)
            {
                return BadRequest(
                    new ErrorDetails {
                        Message = "input parameters cannot be null or 0",
                        Code = (int)ErrorCodes.InvalidInputParameters }
                    );
            }

            object result;

            try
            {
                result = Ok(DataProcessor.Populate(portfolioCount, accountCount, usersCount));
            }
            catch (CouchbaseException cex)
            {
                result = BadRequest(new ErrorDetails { Code = (int)ErrorCodes.CouchbaseProcessing, Message = cex.Message });
            }
            catch (Exception ex)
            {
                ErrorDetails error = new ErrorDetails { Code = (int)ErrorCodes.Unknown, Message = $"Something failed: {ex.Message}" };
                result = BadRequest(error);
            }

            return result;
        }
        
    }
}
