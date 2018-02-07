using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

using DemoService.Data;
using DemoService.Models;

namespace DemoService.Controllers
{
    /// <summary>
    /// actions on accounts
    /// </summary>
    [Route("api/[controller]")]
    public class AccountController : BaseController
    {
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="dataProcessor">the processors to use when performing actions with data</param>
        public AccountController(IDataProcessor dataProcessor)
            :base(dataProcessor)
        { }

        /// <summary>
        /// gets the list of accounts for a given portfolio
        /// </summary>
        /// <param name="portfolioName">the name of the portfolio for which to pull accounts</param>
        /// <returns>Returns a list of <see cref="AccountState"/></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Something failed</response>
        [Produces("application/json")]
        [Route("GetAccountsByPortfolioName")]
        [HttpGet]
        [ProducesResponseType(typeof(ErrorDetails), 400)]
        public object GetAccountsByPortfolioName([FromQuery] string portfolioName)
        {
            if (String.IsNullOrEmpty(portfolioName))
            {
                return BadRequest(
                    new ErrorDetails {
                        Message = "portfolioName cannot be null or empty",
                        Code = (int)ErrorCodes.InvalidInputParameters}
                    );
            }

            object result = null;
            try
            {
                result = Ok(DataProcessor.GetAccountsByPortfolioName(portfolioName));
            }
            catch (Exception ex)
            {
                result = BadRequest(
                    new ErrorDetails {
                        Message = ex.Message,
                        Code = (int)ErrorCodes.CouchbaseProcessing}
                    );
            }
            return result;
        }

        /// <summary>
        /// gets the list of accounts for a given user id
        /// </summary>
        /// <param name="username">the name of the user for which to pull accounts</param>
        /// <returns>Returns a list of <see cref="AccountState"/></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Something failed</response>
        [Produces("application/json")]
        [Route("GetAccountsByUsername")]
        [HttpGet]
        [ProducesResponseType(typeof(ErrorDetails), 400)]
        public object GetAccountsByUsername([FromQuery] string username)
        {
            if (String.IsNullOrEmpty(username))
            {
                return BadRequest(
                    new ErrorDetails
                    {
                        Message = "invalid or empty username",
                        Code = (int)ErrorCodes.InvalidInputParameters
                    }
                    );
            }

            object result = null;
            try
            {
                result = Ok(DataProcessor.GetAccountsByUsername(username));
            }
            catch (Exception ex)
            {
                result = BadRequest(
                    new ErrorDetails
                    {
                        Message = ex.Message,
                        Code = (int)ErrorCodes.CouchbaseProcessing
                    }
                    );
            }
            return result;
        }
    }
}
