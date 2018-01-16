using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoService.Data
{
    /// <summary>
    /// contract for data processing functions
    /// </summary>
    public interface IDataProcessor
    {
        /// <summary>
        /// adds interrelated users, accounts, and portfolios to the the proper buckets so I can test this thing
        /// </summary>
        string Populate(int portfolioCount, int accountCount, int usersCount);

        /// <summary>
        /// authenticates a user
        /// </summary>
        object AuthenticateUser(string username, string password);

        /// <summary>
        /// get all portfolios 
        /// </summary>
        object GetPortfolios();

        /// <summary>
        /// get all portfolios by aggregating account data
        /// </summary>
        object GetPortfoliosByAggregate();

        /// <summary>
        /// gets the list of accounts for a given portfolio id
        /// </summary>
        object GetAccountsByPortfolioId(string portfolioId);

        /// <summary>
        /// gets a list of accounts for a given user id
        /// </summary>
        object GetAccountsByUserId(int userId);
    }
}
