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
        private readonly string SELECTACCOUNT = "select id, portfolioID, currentBalance, accountStatus, asOfDate, lastPaymentDate, lastPaymentAmount, daysDelinquent, userID";
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
        /// adds interrelated user, portfolio, and account records to the couchbase buckets
        /// </summary>
        /// <param name="portfolioCount">the number of portfolio records to create</param>
        /// <param name="accountCount">the number of account records to create</param>
        /// <param name="usersCount">the number of user records to create</param>
        /// <returns>Returns "done" when complete</returns>
        public string Populate(int portfolioCount, int accountCount, int usersCount)
        {
            string result = "done";

            // create a list of portfolios
            Dictionary<int, PortfolioState> portfolios = new Dictionary<int, PortfolioState>();
            for (int p=1; p <= portfolioCount; p++)
            {
                portfolios.Add(p, PortfolioState.Create(p));
            }

            // add accounts, updating the portfolio list as we go
            List<AccountState> accounts = new List<AccountState>();
            int accountId = 0;
            for (int i=0; i < accountCount; i++)
            {
                // create an account (randomly assigns to a portfolio)
                AccountState account = AccountState.Create(accountId++, portfolioCount, usersCount);
                accounts.Add(account);

                // add the account balance to the chosen portfolio, and increment account count for the portfolio
                PortfolioState tmp = portfolios[account.PortfolioID];
                tmp.AccountCount++;
                tmp.TotalBalance += account.CurrentBalance;

                portfolios[account.PortfolioID] = tmp;
            }

            #region synchronous add to couchbase

            try
            {
                // add users
                AddUsers(usersCount);

                // now add all accounts
                accounts.ForEach(account => AddAccount(account));

                // now add all portfolioState records
                foreach (PortfolioState portfolio in portfolios.Values)
                {
                    AddPortfolioState(portfolio);
                }
            }
            catch (Exception ex)
            {
                throw new CouchbaseException($"Populate failed: {ex.Message}", ex);
            }

            #endregion


            return result;
        }

        /// <summary>
        /// gets all portfolios from the bucket
        /// </summary>
        /// <returns>Returns a list of portfolios as an object</returns>
        public object GetPortfolios()
        {
            string name = CouchbaseConfigManager.Instance.PortfolioBucketName;
            string query = $"select id, accountCount, name, totalBalance, asOfDate, debug from {name}";

            return DataClient.ExecuteQuery(name, query);
        }

        /// <summary>
        /// gets all portfolios using the aggregated data from the account bucket
        /// </summary>
        /// <returns>Returns a list of portfolios as an object</returns>
        public object GetPortfoliosByAggregate()
        {
            string name = CouchbaseConfigManager.Instance.AccountBucketName;
            string query = $"select portfolioID as id, TRUNC(SUM(currentBalance),2) as totalBalance, \"Portfolio \" || TOSTRING(portfolioID) as name, COUNT(id) as accountCount  from {name} GROUP BY portfolioID";

            return DataClient.ExecuteQuery(name, query);
        }

        /// <summary>
        /// gets all account for a given portfolio 
        /// </summary>
        /// <param name="portfolioId">the id of the portfolio to retrieve accounts for</param>
        /// <returns>Returns a list of accounts</returns>
        public object GetAccountsByPortfolioId(string portfolioId)
        {
            if (String.IsNullOrEmpty(portfolioId))
            {
                throw new ArgumentException("invalid or null portfolio id");
            }

            string name = CouchbaseConfigManager.Instance.AccountBucketName;
            string query = $"{SELECTACCOUNT} from {name} WHERE portfolioID = {portfolioId}";

            return DataClient.ExecuteQuery(name, query);
        }

        /// <summary>
        /// gets all account for a given user 
        /// </summary>
        /// <param name="userId">the id of the user to retrieve accounts for</param>
        /// <returns>Returns a list of accounts</returns>
        public object GetAccountsByUserId(int userId)
        {
            string name = CouchbaseConfigManager.Instance.AccountBucketName;
            string query = $"{SELECTACCOUNT} from {name} WHERE userID = {userId}";

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
            string query = $"select id, username, lastLogin from {name} where username = '{username}' and pwd = '{password}'";

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
