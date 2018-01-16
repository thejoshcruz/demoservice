using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Couchbase.N1QL;

namespace DemoService.Data
{
    /// <summary>
    /// things you can do with a data client
    /// </summary>
    public interface IDataClient
    {
        List<dynamic> ExecuteQuery(string name, string query);

        List<dynamic> ExecuteQuery(string name, IQueryRequest query);

        void Upsert(string name, object content);
    }
}
