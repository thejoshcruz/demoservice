using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoService.Exceptions
{
    /// <summary>
    /// exception type for configuration problems
    /// </summary>
    public class ConfigurationException : ApplicationException
    {
        /// <summary>
        /// creates exception with the given message and inner exception
        /// </summary>
        /// <param name="message">the message to include</param>
        /// <param name="innerException">the inner exception of the new exception</param>
        public ConfigurationException(string message, Exception innerException) 
            : base(message, innerException)
        { }

        /// <summary>
        /// creates exception with the given message
        /// </summary>
        /// <param name="message">the message to include</param>
        public ConfigurationException(string message)
            : base(message)
        { }
    }
}
