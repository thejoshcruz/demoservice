using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoService.Configuration
{
    /// <summary>
    /// contains the singleton for the couchbase config. logic is elsewhere so it can be tested
    /// </summary>
    public class CouchbaseConfigManager
    {
        private static readonly CouchbaseConfig _instance = new CouchbaseConfig();

        /// <summary>
        /// singleton instance
        /// </summary>
        public static CouchbaseConfig Instance
        {
            get { return _instance; }
        }
    }
}
