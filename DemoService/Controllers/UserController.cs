using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using DemoService.Data;
using DemoService.Exceptions;
using DemoService.Models;

namespace DemoService.Controllers
{
    /// <summary>
    /// actions on a user
    /// </summary>
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="dataProcessor">the processors to use when performing actions with data</param>
        public UserController(IDataProcessor dataProcessor)
            :base(dataProcessor)
        { }

        /// <summary>
        /// authenticates a user with the given credentials
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /Authenticate
        ///     {
        ///       "username": "admin",
        ///       "password": "emptyhahaha"
        ///     }
        ///     
        /// </remarks>
        /// <param name="username">the username</param>
        /// <param name="password">the password</param>
        /// <returns>Returns the user profile</returns>
        [Route("Authenticate")]
        [HttpPost]
        public object Authenticate(string username, string password)
        {
            if (String.IsNullOrEmpty(username) 
                || String.IsNullOrEmpty(password))
            {
                return BadRequest(
                    new ErrorDetails
                    {
                        Code = (int)ErrorCodes.InvalidInputParameters,
                        Message = "invalid credentials"
                    }
                    );
            }

            object result = null;
            try
            {
                result = Ok(DataProcessor.AuthenticateUser(username, password));
            }
            catch (CouchbaseException cex)
            {
                result = BadRequest(
                    new ErrorDetails {
                        Code = (int)ErrorCodes.CouchbaseProcessing,
                        Message = cex.Message }
                    );
            }
            catch (Exception ex)
            {
                result = BadRequest(
                    new ErrorDetails
                    {
                        Message = $"Something failed: {ex.Message}",
                        Code = (int)ErrorCodes.Unknown
                    }
                    );
            }
            return result;
        }

    }
}
