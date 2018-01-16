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
        /// gets the list of accounts for a given portfolio id
        /// </summary>
        /// <param name="portfolioId">the id of the portfolio for which to pull accounts</param>
        /// <returns>Returns a list of <see cref="AccountState"/></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Something failed</response>
        [Produces("application/json")]
        [Route("GetAccountsByPortfolioId")]
        [HttpGet]
        [ProducesResponseType(typeof(ErrorDetails), 400)]
        public object GetAccountsByPortfolioId([FromQuery] string portfolioId)
        {
            if (String.IsNullOrEmpty(portfolioId))
            {
                return BadRequest(
                    new ErrorDetails {
                        Message = "portfolioId cannot be null or empty",
                        Code = (int)ErrorCodes.InvalidInputParameters}
                    );
            }

            object result = null;
            try
            {
                result = Ok(DataProcessor.GetAccountsByPortfolioId(portfolioId));
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
        /// <param name="userId">the id of the user for which to pull accounts</param>
        /// <returns>Returns a list of <see cref="AccountState"/></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Something failed</response>
        [Produces("application/json")]
        [Route("GetAccountsByUserId")]
        [HttpGet]
        [ProducesResponseType(typeof(ErrorDetails), 400)]
        public object GetAccountsByUserId([FromQuery] int userId)
        {
            if (userId <= 0)
            {
                return BadRequest(
                    new ErrorDetails
                    {
                        Message = "userId cannot be <= 0",
                        Code = (int)ErrorCodes.InvalidInputParameters
                    }
                    );
            }

            object result = null;
            try
            {
                result = Ok(DataProcessor.GetAccountsByUserId(userId));
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
