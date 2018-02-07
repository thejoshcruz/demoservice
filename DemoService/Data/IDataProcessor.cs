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
        /// authenticates a user
        /// </summary>
        object AuthenticateUser(string username, string password);

        /// <summary>
        /// get all portfolios 
        /// </summary>
        object GetPortfolios();
        
        /// <summary>
        /// gets the list of accounts for a given portfolio
        /// </summary>
        object GetAccountsByPortfolioName(string portfolioName);

        /// <summary>
        /// gets a list of accounts for a given username
        /// </summary>
        object GetAccountsByUsername(string username);
    }
}
