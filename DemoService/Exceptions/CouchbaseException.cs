using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoService.Exceptions
{
    /// <summary>
    /// default exception type for couchbase operations
    /// </summary>
    public class CouchbaseException : Exception
    {
        /// <summary>
        /// creates exception with the given message and inner exception
        /// </summary>
        /// <param name="message">the message to include</param>
        /// <param name="innerException">the inner exception of the new exception</param>
        public CouchbaseException(string message, Exception innerException) 
            : base(message, innerException)
        { }

        /// <summary>
        /// creates exception with the given message
        /// </summary>
        /// <param name="message">the message to include</param>
        public CouchbaseException(string message)
            : base(message)
        { }
    }
}
