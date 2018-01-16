using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoService.Models
{
    /// <summary>
    /// possible account status
    /// </summary>
    public enum AccountStatus
    {
        /// <summary>
        /// active
        /// </summary>
        Active = 0,

        /// <summary>
        /// not active (aka inactive, pointdexter)
        /// </summary>
        Inactive,

        /// <summary>
        /// someone gone and canceled this account, pa!
        /// </summary>
        Canceled
    }


    /// <summary>
    /// possible error codes
    /// </summary>
    public enum ErrorCodes
    {
        /// <summary>
        /// nobody knows what happened
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// something broke while processing with couchbase
        /// </summary>
        CouchbaseProcessing,

        /// <summary>
        /// invalid input values
        /// </summary>
        InvalidInputParameters
    }
}
