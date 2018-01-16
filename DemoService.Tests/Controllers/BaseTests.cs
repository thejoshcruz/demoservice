using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;

using DemoService.Models;

namespace DemoService.Tests.Controllers
{
    /// <summary>
    /// base class for tests; provides useful functions and properties for reuse by derived classes
    /// </summary>
    public class BaseTests
    {
        /// <summary>
        /// parse the error code from a Bad request reponse object
        /// </summary>
        /// <param name="result">the bad request response from the service</param>
        /// <returns>Returns the error code from the response; -1 if it fails to parse</returns>
        public int ParseBadRequestForErrorCode(object result)
        {
            int code = -1;
            if (result is BadRequestObjectResult)
            {
                object error = ((BadRequestObjectResult)result).Value;
                if (error is ErrorDetails)
                {
                    code = ((ErrorDetails)error).Code;
                }
            }
            return code;
        }
    }
}
