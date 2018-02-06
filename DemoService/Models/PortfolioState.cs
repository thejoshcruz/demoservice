using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoService.Models
{
    /// <summary>
    /// portfolio state info
    /// </summary>
    public class PortfolioState
    {
        /// <summary>
        /// portfolio number
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// name of the portfolio
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// total balance of the portfolio
        /// </summary>
        public decimal TotalBalance { get; set; }

        /// <summary>
        /// number of accounts in the portfolio
        /// </summary>
        public int AccountCount { get; set; }

        /// <summary>
        /// as of date for this state of the portfolio
        /// </summary>
        public DateTime AsOfDate { get; set; }

        /// <summary>
        /// creates a random portfolio state instance
        /// </summary>
        /// <param name="number">the number of the new instance</param>
        public static PortfolioState Create(string number)
        {
            PortfolioState p = new PortfolioState
            {
                Number = number,
                Name = "Portfolio " + number,
                AsOfDate = DateTime.Now,
                AccountCount = 0,
                TotalBalance = decimal.Zero
            };

            return p;
        }
    }
}
