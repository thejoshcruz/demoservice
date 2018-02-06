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
    /// actions on portfolios
    /// </summary>
    [Route("api/[controller]")]
    public class PortfolioController : BaseController
    {
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="dataProcessor">the processors to use when performing actions with data</param>
        public PortfolioController(IDataProcessor dataProcessor)
            :base(dataProcessor)
        { }


        /// <summary>
        /// get all portfolios 
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /GetPortfolios
        ///     {
        ///     }
        ///     
        /// </remarks>
        /// <returns>Returns a list of portfolios</returns>
        /// <response code="200">Success</response>
        /// <response code="201">Demonstrating how to show more</response>
        /// <response code="400">Something is null</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(object), 201)]
        [ProducesResponseType(typeof(ErrorDetails), 400)]
        [Route("GetPortfolios")]
        [HttpGet]
        public object GetPortfolios()
        {
            object result;
            try
            {
                result = Ok(DataProcessor.GetPortfolios());
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
        
    }
}
