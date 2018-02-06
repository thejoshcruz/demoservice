using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Couchbase;
using Couchbase.Core;
using Couchbase.Configuration.Client;
using Couchbase.Authentication;
using Couchbase.N1QL;

using Newtonsoft.Json;

using DemoService.Configuration;
using DemoService.Exceptions;
using DemoService.Models;

namespace DemoService.Data
{
    /// <summary>
    /// provides functions to perform on the couchbase cluster
    /// </summary>
    public class CouchbaseProcessor : IDataProcessor
    {
        private readonly string SELECTACCOUNT = "select AccountNumber, PortfolioNumber, CurrentBalance, AccountStatus, AsOfDate, LastPaymentDate, LastPaymentAmount, DaysDelinquent, Username, AccountInventory";
        private IDataClient DataClient;
        
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="dataClient">data client to use for data operations</param>
        public CouchbaseProcessor(IDataClient dataClient)
        {
            DataClient = dataClient;
        }


        /// <summary>
        /// gets all portfolios from the bucket
        /// </summary>
        /// <returns>Returns a list of portfolios as an object</returns>
        public object GetPortfolios()
        {
            string name = CouchbaseConfigManager.Instance.PortfolioBucketName;
            string query = $"select PortfolioNumber, AccountCount, Name, TotalBalance, AsOfDate, Debug from {name}";

            return DataClient.ExecuteQuery(name, query);
        }

        /// <summary>
        /// gets all account for a given portfolio 
        /// </summary>
        /// <param name="portfolioNumber">the number of the portfolio to retrieve accounts for</param>
        /// <returns>Returns a list of accounts</returns>
        public object GetAccountsByPortfolioNumber(string portfolioNumber)
        {
            if (String.IsNullOrEmpty(portfolioNumber))
            {
                throw new ArgumentException("invalid or null portfolio number");
            }

            string name = CouchbaseConfigManager.Instance.AccountBucketName;
            string query = $"{SELECTACCOUNT} from {name} WHERE PortfolioNumber = '{portfolioNumber}'";

            return DataClient.ExecuteQuery(name, query);
        }

        /// <summary>
        /// gets all account for a given user 
        /// </summary>
        /// <param name="username">the name of the user to retrieve accounts for</param>
        /// <returns>Returns a list of accounts</returns>
        public object GetAccountsByUsername(string username)
        {
            if (String.IsNullOrEmpty(username))
            {
                throw new ArgumentException("invalid or null username");
            }

            string name = CouchbaseConfigManager.Instance.AccountBucketName;
            string query = $"{SELECTACCOUNT} from {name} WHERE Username = '{username}'";

            return DataClient.ExecuteQuery(name, query);
        }

        /// <summary>
        /// authenticates a user
        /// </summary>
        /// <param name="username">the username to authenticate</param>
        /// <param name="password">the password to use in authentication</param>
        /// <returns></returns>
        public object AuthenticateUser(string username, string password)
        {
            if (String.IsNullOrEmpty(username)
                || String.IsNullOrEmpty(password))
            {
                throw new ArgumentException("invalid or empty credentials");
            }

            string name = CouchbaseConfigManager.Instance.UserBucketName;
            string query = $"select username, lastLogin from {name} where username = '{username}' and pwd = '{password}'";

            List<dynamic> results = DataClient.ExecuteQuery(name, query);
            if (results.Count != 1)
            {
                throw new CouchbaseException("invalid login");
            }

            return results.First();
        }
        



        private void AddAccount(AccountState account)
        {
            DataClient.Upsert(CouchbaseConfigManager.Instance.AccountBucketName, account);
        }

        private void AddPortfolioState(PortfolioState portfolio)
        {
            DataClient.Upsert(CouchbaseConfigManager.Instance.PortfolioBucketName, portfolio);
        }

        private void AddUsers(int count)
        {
            List<User> users = new List<User>
            {
                new User { Username = "admin", Password = "admin", LastLogin = DateTime.Now, ID = 0 }
            };

            for (int i=1; i <= count; i++)
            {
                users.Add(new User { ID = i, Username = $"user{i}", Password = "password", LastLogin = DateTime.Now });
            }

            users.ForEach(user => DataClient.Upsert(CouchbaseConfigManager.Instance.UserBucketName, user));
        }
    }
}
