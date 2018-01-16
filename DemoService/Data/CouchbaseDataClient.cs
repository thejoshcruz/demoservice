using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Couchbase;
using Couchbase.Core;
using Couchbase.N1QL;

namespace DemoService.Data
{
    public class CouchbaseDataClient : IDataClient
    {
        public List<dynamic> ExecuteQuery(string name, string query)
        {
            return ClusterHelper
                .GetBucket(name)
                .Query<dynamic>(query)
                .Rows;
        }

        public List<dynamic> ExecuteQuery(string name, IQueryRequest query)
        {
            return ClusterHelper
                .GetBucket(name)
                .Query<dynamic>(query)
                .Rows;
        }

        public void Upsert(string name, object content)
        {
            var document = new Document<dynamic>
            {
                Id = DateTime.Now.ToString("MMddHHmmssfff"),
                Content = content
            };

            IBucket bucket = ClusterHelper.GetBucket(name);
            var upsert = bucket.Upsert(document);
            if (!upsert.Success)
            {
                throw new Exception("failed to upsert record");
            }
        }
    }
}
