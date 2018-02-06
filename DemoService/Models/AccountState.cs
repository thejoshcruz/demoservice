using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DemoService.Models
{
    /// <summary>
    /// account details as of certain date
    /// </summary>
    public class AccountState
    {
        /// <summary>
        /// account identifier/number
        /// </summary>
        [Required]
        public string AccountNumber { get; set; }

        /// <summary>
        /// portfolio identifier/number
        /// </summary>
        public string PortfolioNumber { get; set; }

        /// <summary>
        /// the name of the user to which this account belongs
        /// </summary>
        public string Username { get; set; }
        
        /// <summary>
        /// current balance of the account
        /// </summary>
        public decimal CurrentBalance { get; set; }

        /// <summary>
        /// status of the account
        /// </summary>
        public AccountStatus AccountStatus { get; set; }
        
        /// <summary>
        /// date this record represents
        /// </summary>
        public DateTime AsOfDate { get; set; }

        /// <summary>
        /// last payment date
        /// </summary>
        public DateTime LastPaymentDate { get; set; }

        /// <summary>
        /// amount of last payment
        /// </summary>
        public decimal LastPaymentAmount { get; set; }

        /// <summary>
        /// number of days delinquent
        /// </summary>
        public int DaysDelinquent { get; set; }

        /// <summary>
        /// description of the account inventory
        /// </summary>
        public string AccountInventory { get; set; }


        /// <summary>
        /// creates a random instance of AccountState
        /// </summary>
        /// <param name="id">the id of the new instance</param>
        /// <param name="maxPortfolioId">the maximum portfolio id to use when creating the instance</param>
        /// <param name="maxUserId">the maximum user id to use when creating the instance</param>
        /// <returns></returns>
        public static AccountState Create(int id, int maxPortfolioId, int maxUserId)
        {
            // to determine days delinq, let's do this:
            // generate a last payment date between DateTime.Today and random (0-180) days from today
            // subtract last payment date from 30 prior to today = days delinq

            Random random = new Random();

            // generate last payment date
            DateTime lastPayDate = DateTime.Today.Subtract(new TimeSpan(random.Next(0, 180), 0, 0, 0));
            int daysDelinq = DateTime.Today.Subtract(new TimeSpan(30, 0, 0, 0)).Subtract(lastPayDate).Days;
            if (daysDelinq < 0)
            {
                // no negative days delinq
                daysDelinq = 0;
            }

            AccountState account = new AccountState
            {
                AccountNumber = (10000000000 + id).ToString(),
                PortfolioNumber = "Portfolio" + random.Next(1, maxPortfolioId + 1),
                Username = "User" + random.Next(1, maxUserId + 1),
                CurrentBalance = Math.Round(2000 * (decimal)random.NextDouble(), 2),
                LastPaymentAmount = Math.Round(600 * (decimal)random.NextDouble(), 2),
                LastPaymentDate = lastPayDate,
                DaysDelinquent = daysDelinq,
                AccountStatus = (AccountStatus)random.Next(0, 2),
                AsOfDate = DateTime.Now,
                AccountInventory = "Inventory" + id
            };
            return account;
        }
    }
}
