using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoService.Models
{
    /// <summary>
    /// details about an error returned from an incoming request
    /// </summary>
    public class ErrorDetails
    {
        /// <summary>
        /// description of what went wrong
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// the error code 
        /// </summary>
        public int Code { get; set; }
    }
}
